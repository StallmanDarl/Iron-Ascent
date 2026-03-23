using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public static PlayerHealth Instance;

    public delegate void OnHealthChanged(int current, int max);
    public static event OnHealthChanged onHealthChanged;

    void Awake()
    {
        // If another instance exists and it's not this one, destroy this duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // This becomes the singleton instance
        Instance = this;

        // Persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
    }

    void Die()
    {
        Debug.Log("Player Died");

        // Reset run progress
        RunManager.Instance.ResetRunProgress();

        // GoTo starting scene
        TransitionManager.Instance.LoadSceneWithSpawn("HomeBase", "Respawn");
        currentHealth = maxHealth;
    }
}