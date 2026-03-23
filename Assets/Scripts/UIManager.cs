using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;

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
}