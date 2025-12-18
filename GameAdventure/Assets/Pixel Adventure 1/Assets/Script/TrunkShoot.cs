using UnityEngine;

public class TrunkShoot : MonoBehaviour
{
    public GameObject bulletPrefab;     // Drag Bullet prefab vào
    public Transform firePoint;         // Vị trí bắn đạn
    public float shootInterval = 2f;    // Thời gian giữa mỗi lần bắn
    public bool facingRight = false;    // Hướng bắn
    public Animator animator;

    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        // Spawn viên đạn
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        // Nếu viên đạn hoặc trunk có collider, bỏ va chạm giữa chúng để viên đạn không tự huỷ ngay khi spawn
        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Collider2D ownerCol = GetComponent<Collider2D>();
        if (bulletCol != null && ownerCol != null)
        {
            Physics2D.IgnoreCollision(bulletCol, ownerCol);
        }
        // Hủy viên đạn sau 6s nếu không va chạm gì để dọn dẹp
        Destroy(bullet, 6f);
        Debug.Log("TrunkShoot: Đã tạo bullet tại vị trí " + firePoint.position);
        animator.SetTrigger("Attack");
        // Lấy script của bullet để set hướng
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.direction = facingRight ? Vector2.right : Vector2.left;
        }
        Debug.Log("Shoot() được gọi!");

        Debug.Log("bulletPrefab = " + bulletPrefab);
        Debug.Log("firePoint = " + firePoint);
    }
}
