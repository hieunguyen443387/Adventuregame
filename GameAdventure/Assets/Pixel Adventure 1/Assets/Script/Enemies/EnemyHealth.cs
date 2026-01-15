using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 5;
    protected int currentHealth;   // 👈 protected để class con dùng được

    public Animator animator;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // 👇 cho phép override
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " mất " + damage + " máu, còn: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        animator.SetTrigger("Die");
    }

    // GỌI TỪ ANIMATION EVENT
    public void DestroyEnemy()
    {
        Debug.Log(gameObject.name + " bị destroy");
        Destroy(gameObject);
    }
}