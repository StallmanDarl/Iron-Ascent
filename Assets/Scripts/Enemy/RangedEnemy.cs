using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    public float projectileSpeed = 15f;
    public float sightRange = 20f;

    private float lastAttackTime;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        //In Sight?
        if (CanSeePlayer())
        {
            //Aim !
            Vector3 targetDir = player.transform.position - transform.position;
            targetDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * 5f);

            //Fire !!!
            Attack();
        }
    }

    bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= sightRange)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            if (Physics.Raycast(firePoint.position, directionToPlayer, out RaycastHit hit, sightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            // Create bullet and aim it directly at player's current position
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Fire in the direction the enemy is currently facing
                rb.linearVelocity = transform.forward * projectileSpeed;
            }
        }
    }
}
