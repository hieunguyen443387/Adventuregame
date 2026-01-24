using UnityEngine;

public class RockHealth : EnemyHealth
{
    [Header("Rock Split Settings")]
    public GameObject nextRockPrefab;
    public int spawnCount = 1;
    public float spawnOffset = 0.3f;

    protected override void Die()
    {
        // CHỈ chạy animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            // Nếu không có anim thì spawn + destroy ngay
            SpawnNextRock();
            Destroy(gameObject);
        }
    }

    // 👇 GỌI TỪ ANIMATION EVENT (frame gần cuối anim Die)
    public void SpawnNextRock()
    {
        if (nextRockPrefab == null) return;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnOffset;
            Instantiate(
                nextRockPrefab,
                (Vector2)transform.position + offset,
                Quaternion.identity
            );
        }
    }

    // 👇 GỌI TỪ ANIMATION EVENT (frame cuối cùng)
    public new void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
