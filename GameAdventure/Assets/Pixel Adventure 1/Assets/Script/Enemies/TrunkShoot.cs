using UnityEngine;

public class TrunkShoot : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject bulletPrefab;   // Prefab viên đạn
    public Transform firePoint;       // Điểm bắn
    public bool facingRight = false;  // Hướng bắn

    [Header("Attack Timing")]
    public float shootInterval = 2f;  // Thời gian giữa 2 lần tấn công

    private float timer;
    private Animator animator;
    private Collider2D ownerCol;

    void Start()
    {
        animator = GetComponent<Animator>();
        ownerCol = GetComponent<Collider2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            animator.SetTrigger("Attack"); // Chỉ trigger anim
            timer = 0f;
        }
    }

    // ⚠ HÀM NÀY ĐƯỢC GỌI TỪ ANIMATION EVENT (frame 0.07s)
    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Ignore collision giữa trunk và bullet
        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        if (bulletCol != null && ownerCol != null)
        {
            Physics2D.IgnoreCollision(bulletCol, ownerCol);
        }

        // Set hướng bay
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.direction = facingRight ? Vector2.right : Vector2.left;
        }

        // Auto destroy nếu bay hoài không trúng gì
        Destroy(bullet, 6f);

        Debug.Log("🔥 Bullet fired from Animation Event");
    }
}