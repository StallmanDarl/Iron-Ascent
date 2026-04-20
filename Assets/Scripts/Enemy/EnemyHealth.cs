using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("HealthBar")]
    public Slider healthBar;
    public int maxHealth = 50;
    private int currentHealth;

    private Animator animator;

    public System.Action OnEnemyDeath;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.value = currentHealth;
        Debug.Log("Enemy took damage: " + amount);

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnEnemyDeath?.Invoke();
        // Later: play animation
        // Later: drop loot

        Destroy(gameObject);
    }
}