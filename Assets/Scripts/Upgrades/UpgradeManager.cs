using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    [Serializable]
    public class UpgradeChoice
    {
        public string category;
        public string title;
        public string description;
        public Sprite icon;
        public Action apply;
    }

    public static UpgradeManager Instance;

    [Header("Run Upgrade Values")]
    [SerializeField] int runHealthBonusPerPick = 25;
    [SerializeField] int runDamageBonusPerPick = 10;
    [SerializeField] int runStaminaBonusPerPick = 20;
    [SerializeField] float runMoveSpeedBonusPerPick = 0.75f;
    [SerializeField] float runSprintSpeedBonusPerPick = 0.9f;
    [SerializeField] int runHealAmount = 35;

    [Header("Optional Card Icons")]
    [SerializeField] Sprite healthIcon;
    [SerializeField] Sprite damageIcon;
    [SerializeField] Sprite staminaIcon;
    [SerializeField] Sprite mobilityIcon;
    [SerializeField] Sprite healIcon;

    int baseMaxHealth;
    int baseDamage;
    int baseMaxStamina;
    float baseMoveSpeed;
    float baseSprintSpeed;
    bool baseStatsCaptured;

    int runHealthBonus;
    int runDamageBonus;
    int runStaminaBonus;
    float runMoveSpeedBonus;
    float runSprintSpeedBonus;

    bool metaCardsShownThisScene;
    bool selectionOpen;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        Time.timeScale = 1f;
    }

    public static void EnsureExists()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject obj = new GameObject("UpgradeManager");
        obj.AddComponent<UpgradeManager>();
    }

    public bool IsShowingUpgradeCards()
    {
        return selectionOpen;
    }

    public void ResetRunUpgrades()
    {
        runHealthBonus = 0;
        runDamageBonus = 0;
        runStaminaBonus = 0;
        runMoveSpeedBonus = 0f;
        runSprintSpeedBonus = 0f;
        ApplyAllUpgradesToPlayer();
    }

    public void ShowArenaUpgradeChoices(Action onResolved)
    {
        Debug.Log("UpgradeManager: Showing Arena Cards");

        if (UIManager.Instance == null)
        {
            Debug.Log("FAILED: UIManager is null");
            onResolved?.Invoke();
            return;
        }

        selectionOpen = true;
        Time.timeScale = 0f;

        UIManager.Instance.ShowUpgradeChoices(
            "Arena Cleared",
            "Choose one run upgrade before moving into the next arena.",
            PickUniqueChoices(BuildArenaUpgradePool(), 3),
            ResolveChoice(onResolved)
        );
    }

    public void ShowMetaUpgradeChoices()
    {
        if (selectionOpen || UIManager.Instance == null)
        {
            if (RunManager.Instance != null)
            {
                RunManager.Instance.MetaUpgradeCollected();
            }
            return;
        }

        selectionOpen = true;
        Time.timeScale = 0f;

        UIManager.Instance.ShowUpgradeChoices(
            "Meta Upgrade",
            "Choose one permanent upgrade for future runs.",
            BuildMetaUpgradeChoices(),
            ResolveChoice(() =>
            {
                if (RunManager.Instance != null)
                {
                    RunManager.Instance.MetaUpgradeCollected();
                }
            })
        );
    }

    Action<UpgradeChoice> ResolveChoice(Action onResolved)
    {
        return choice =>
        {
            Time.timeScale = 1f;
            selectionOpen = false;

            choice.apply?.Invoke();
            ApplyAllUpgradesToPlayer();

            SkyboxManager.Instance.ApplySkybox();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideUpgradeChoices();
            }

            onResolved?.Invoke();
        };
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        metaCardsShownThisScene = false;
        ApplyAllUpgradesToPlayer();
    }

    List<UpgradeChoice> BuildArenaUpgradePool()
    {
        return new List<UpgradeChoice>
        {
            new UpgradeChoice
            {
                category = "Run Upgrade",
                title = "Vanguard Plating",
                description = "+" + runHealthBonusPerPick + " max health and heal to match the gain.",
                icon = healthIcon,
                apply = () =>
                {
                    runHealthBonus += runHealthBonusPerPick;

                    if (PlayerHealth.Instance != null)
                    {
                        PlayerHealth.Instance.currentHealth += runHealthBonusPerPick;
                    }
                }
            },
            new UpgradeChoice
            {
                category = "Run Upgrade",
                title = "Sharpened Edge",
                description = "+" + runDamageBonusPerPick + " attack damage for the rest of this run.",
                icon = damageIcon,
                apply = () => runDamageBonus += runDamageBonusPerPick
            },
            new UpgradeChoice
            {
                category = "Run Upgrade",
                title = "Reservoir",
                description = "+" + runStaminaBonusPerPick + " max stamina for longer fights.",
                icon = staminaIcon,
                apply = () =>
                {
                    runStaminaBonus += runStaminaBonusPerPick;

                    if (PlayerStamina.Instance != null)
                    {
                        PlayerStamina.Instance.currentStamina += runStaminaBonusPerPick;
                    }
                }
            },
            new UpgradeChoice
            {
                category = "Run Upgrade",
                title = "Quickstep Greaves",
                description = "Increase move speed and sprint speed for the rest of the run.",
                icon = mobilityIcon,
                apply = () =>
                {
                    runMoveSpeedBonus += runMoveSpeedBonusPerPick;
                    runSprintSpeedBonus += runSprintSpeedBonusPerPick;
                }
            },
            new UpgradeChoice
            {
                category = "Run Upgrade",
                title = "Second Wind",
                description = "Recover " + runHealAmount + " health immediately.",
                icon = healIcon,
                apply = () =>
                {
                    if (PlayerHealth.Instance != null)
                    {
                        PlayerHealth.Instance.Heal(runHealAmount);
                    }
                }
            }
        };
    }

    List<UpgradeChoice> BuildMetaUpgradeChoices()
    {
        return new List<UpgradeChoice>
        {
            new UpgradeChoice
            {
                category = "Prefab Upgrade",
                title = "Titan Frame",
                description = "Permanently increase player max health on future runs.",
                icon = healthIcon,
                apply = () => ApplyMetaUpgrade("Health")
            },
            new UpgradeChoice
            {
                category = "Prefab Upgrade",
                title = "Tempered Edge",
                description = "Permanently increase player attack damage on future runs.",
                icon = damageIcon,
                apply = () => ApplyMetaUpgrade("Damage")
            },
            new UpgradeChoice
            {
                category = "General Upgrade",
                title = "Deep Reserves",
                description = "Permanently increase player stamina on future runs.",
                icon = staminaIcon,
                apply = () => ApplyMetaUpgrade("Stamina")
            }
        };
    }

    void ApplyMetaUpgrade(string upgradeType)
    {
        GameManager.EnsureExists();

        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.ApplyMetaUpgrade(upgradeType);
        ApplyAllUpgradesToPlayer();
    }

    List<UpgradeChoice> PickUniqueChoices(List<UpgradeChoice> pool, int count)
    {
        List<UpgradeChoice> available = new List<UpgradeChoice>(pool);
        List<UpgradeChoice> picked = new List<UpgradeChoice>();
        int targetCount = Mathf.Min(count, available.Count);

        while (picked.Count < targetCount)
        {
            int index = UnityEngine.Random.Range(0, available.Count);
            picked.Add(available[index]);
            available.RemoveAt(index);
        }

        return picked;
    }

    void CaptureBaseStatsIfNeeded()
    {
        if (baseStatsCaptured)
        {
            return;
        }

        if (PlayerHealth.Instance == null || PlayerStamina.Instance == null)
        {
            return;
        }

        PlayerAttackTester attack = FindFirstObjectByType<PlayerAttackTester>();
        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();

        if (attack == null || movement == null)
        {
            return;
        }

        baseMaxHealth = PlayerHealth.Instance.maxHealth;
        baseDamage = attack.damage;
        baseMaxStamina = PlayerStamina.Instance.maxStamina;
        baseMoveSpeed = movement.moveSpeed;
        baseSprintSpeed = movement.sprintSpeed;
        baseStatsCaptured = true;
    }

    void ApplyAllUpgradesToPlayer()
    {
        CaptureBaseStatsIfNeeded();

        if (!baseStatsCaptured)
        {
            return;
        }

        GameManager.EnsureExists();

        PlayerAttackTester attack = FindFirstObjectByType<PlayerAttackTester>();
        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();

        if (PlayerHealth.Instance == null || PlayerStamina.Instance == null || attack == null || movement == null)
        {
            return;
        }

        int targetMaxHealth = baseMaxHealth + runHealthBonus + GameManager.Instance.GetPermanentHealthBonus();
        int currentHealthBefore = PlayerHealth.Instance.currentHealth;
        int healthDelta = targetMaxHealth - PlayerHealth.Instance.maxHealth;
        PlayerHealth.Instance.maxHealth = targetMaxHealth;
        PlayerHealth.Instance.currentHealth = Mathf.Clamp(currentHealthBefore + Mathf.Max(healthDelta, 0), 0, targetMaxHealth);

        attack.damage = baseDamage + runDamageBonus + GameManager.Instance.GetPermanentDamageBonus();

        int targetMaxStamina = baseMaxStamina + runStaminaBonus + GameManager.Instance.GetPermanentStaminaBonus();
        float currentStaminaBefore = PlayerStamina.Instance.currentStamina;
        float staminaDelta = targetMaxStamina - PlayerStamina.Instance.maxStamina;
        PlayerStamina.Instance.maxStamina = targetMaxStamina;
        PlayerStamina.Instance.currentStamina = Mathf.Clamp(currentStaminaBefore + Mathf.Max(staminaDelta, 0f), 0f, targetMaxStamina);

        movement.moveSpeed = baseMoveSpeed + runMoveSpeedBonus;
        movement.sprintSpeed = baseSprintSpeed + runSprintSpeedBonus;
    }
}
