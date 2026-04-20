using UnityEngine;

public class PlayerAttackTester : MonoBehaviour
{
    public int damage = 25;
    public float attackRange = 3f;
    public int staminaCost = 20;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (UpgradeManager.Instance != null && UpgradeManager.Instance.IsShowingUpgradeCards())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Block attack if not enough stamina
            if (!PlayerStamina.Instance.HasStamina(staminaCost))
            {
                return;
            }
            else {
                animator.SetTrigger("Attack");
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