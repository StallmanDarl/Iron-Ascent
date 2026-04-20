using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Permanent upgrades
    public int permanentHealthLevel = 0;
    public int permanentDamageLevel = 0;
    public int permanentStaminaLevel = 0;

    [Header("Permanent Upgrade Values")]
    [SerializeField] int permanentHealthPerLevel = 20;
    [SerializeField] int permanentDamagePerLevel = 5;
    [SerializeField] int permanentStaminaPerLevel = 15;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists forever
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void EnsureExists()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject obj = new GameObject("GameManager");
        obj.AddComponent<GameManager>();
    }

    // Called when player earns meta upgrade
    public void ApplyMetaUpgrade(string upgradeType)
    {
        switch (upgradeType)
        {
            case "Health":
                permanentHealthLevel++;
                break;
            case "Damage":
                permanentDamageLevel++;
                break;
            case "Stamina":
                permanentStaminaLevel++;
                break;
        }

        Debug.Log("Permanent Upgrade Applied: " + upgradeType);
    }

    public int GetPermanentHealthBonus()
    {
        return permanentHealthLevel * permanentHealthPerLevel;
    }

    public int GetPermanentDamageBonus()
    {
        return permanentDamageLevel * permanentDamagePerLevel;
    }

    public int GetPermanentStaminaBonus()
    {
        return permanentStaminaLevel * permanentStaminaPerLevel;
    }
}
