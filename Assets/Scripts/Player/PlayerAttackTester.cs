using UnityEngine;

public class PlayerAttackTester : MonoBehaviour
{
    public int damage = 25;
    public float attackRange = 3f;
    public int staminaCost = 20;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Block attack if not enough stamina
            if (!PlayerStamina.Instance.HasStamina(staminaCost))
            {
                return;
            }

            PlayerStamina.Instance.UseStamina(staminaCost);

            // Later replace with attack animations and hitboxes
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}