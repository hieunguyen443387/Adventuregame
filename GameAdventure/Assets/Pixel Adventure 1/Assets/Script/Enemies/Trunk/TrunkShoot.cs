using UnityEngine;

public class TrunkShoot : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Detect Settings")]
    public float detectRange = 8f;

    [Header("Attack Timing")]
    public float shootInterval = 2f;

    private float timer;
    private Animator animator;
    private Collider2D ownerCol;

    private bool isFacingRight = false;
    private bool playerDetected = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        ownerCol = GetComponent<Collider2D>();

        // 🔍 Tự tìm Player
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            playerDetected = true;
            HandleFlip();

            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                animator.SetTrigger("Attack");
                timer = 0f;
            }
        }
        else
        {
            playerDetected = false;
            timer = 0f;
        }
    }

    void HandleFlip()
    {
        float xDiff = player.position.x - transform.position.x;

        if (xDiff > 0 && !isFacingRight)
            Flip();
        else if (xDiff < 0 && isFacingRight)
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ⚠ GỌI TỪ ANIMATION EVENT
    public void Shoot()
    {
        if (!playerDetected) return;
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        if (bulletCol != null && ownerCol != null)
            Physics2D.IgnoreCollision(bulletCol, ownerCol);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.direction = isFacingRight ? Vector2.right : Vector2.left;

        Destroy(bullet, 6f);
    }
}
