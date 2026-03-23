using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : MonoBehaviour
{
    public float attackRange = 2f;     // Distance enemy must be to attack
    public float attackCooldown = 2f;  // Time between attacks
    public int damage = 10;            // Damage dealt per attack

    private float lastAttackTime;      // Tracks last attack moment
    private GameObject player;
    private NavMeshAgent agent;

    // Reference to player's health script
    private PlayerHealth playerHealth;

    void Start()
    {
        // Find player by tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Cache NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Cache PlayerHealth component for faster access
        if (player != null)
        {
            // Try multiple ways to find PlayerHealth
            playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth == null)
                playerHealth = player.GetComponentInChildren<PlayerHealth>();
            if (playerHealth == null)
                playerHealth = player.GetComponentInParent<PlayerHealth>();

            if (playerHealth == null)
                Debug.LogError("PlayerHealth NOT FOUND on Player!");
            else
                Debug.Log("PlayerHealth found successfully.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Stop moving when close enough to attack
        if (distance <= attackRange)
        {
            agent.isStopped = true;
            Attack();
        }
        else
        {
            agent.isStopped = false;
        }
    }

    void Attack()
    {
        // Only attack if cooldown passed
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("Enemy Attacks Player!");

            // Later: trigger animation here

            // Damage the player
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player took damage: " + damage);
            }
            else
            {
                Debug.LogError("PlayerHealth still NULL during attack.");
            }
        }
    }
}