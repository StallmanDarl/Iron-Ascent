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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
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
            // Later: reduce player health here
        }
    }
}