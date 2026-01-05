using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " mất " + damage + " máu, còn: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die"); // chỉ chạy animation
    }

    // 👇 HÀM NÀY SẼ ĐƯỢC GỌI TỪ ANIMATION EVENT
    public void DestroyEnemy()
    {
        Debug.Log(gameObject.name + " bị destroy");
        Destroy(gameObject);
    }
}
