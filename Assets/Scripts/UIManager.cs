using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Data.Common;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD Sections")]
    public GameObject gameplayHUD;          // health, stamina, etc.
    public UpgradeOverlayUI upgradeOverlay; // card UI

    [Header("Bars")]
    public Slider healthBar;
    public Slider staminaBar;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    void Start()
    {
        // Ensure correct initial state
        gameplayHUD.SetActive(true);
        upgradeOverlay.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name != "TitleScene")
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    void Update()
    {
        if (PlayerHealth.Instance != null)
        {
            healthBar.maxValue = PlayerHealth.Instance.maxHealth;
            healthBar.value = PlayerHealth.Instance.currentHealth;
        }

        if (PlayerStamina.Instance != null)
        {
            staminaBar.maxValue = PlayerStamina.Instance.maxStamina;
            staminaBar.value = PlayerStamina.Instance.currentStamina;
        }
    }

    // SHOW UPGRADE UI
    public void ShowUpgradeChoices(
        string title,
        string subtitle,
        List<UpgradeManager.UpgradeChoice> choices,
        System.Action<UpgradeManager.UpgradeChoice> onSelected)
    {
        Debug.Log("UI: Showing Upgrade Choices");

        if (upgradeOverlay == null)
        {
            upgradeOverlay = GetComponentInChildren<UpgradeOverlayUI>(true);
        }

        gameplayHUD.SetActive(false);
        upgradeOverlay.Show(title, subtitle, choices, onSelected);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // HIDE UPGRADE UI
    public void HideUpgradeChoices()
    {
        Debug.Log("UI: Hiding Upgrade Choices");

        upgradeOverlay.Hide();
        gameplayHUD.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}