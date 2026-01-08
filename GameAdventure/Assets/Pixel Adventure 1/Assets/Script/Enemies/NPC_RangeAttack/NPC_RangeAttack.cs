using UnityEngine;

public class NPC_RangeAttack : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Detect Settings")]
    public float detectRange = 8f;
    public float maxVerticalDiff = 1f; // chỉ phát hiện khi player cùng mức ngang

    [Header("Attack Timing")]
    public float shootInterval = 2f;
    [Header("Vision")]
    public LayerMask wallLayer;   // 👈 layer của tường

    private float timer;
    private Animator animator;
    private Collider2D ownerCol;

    private bool isFacingRight = false;
    private bool playerDetected = false;
    private PlayerController playerController;
    private PlayerHealth playerHealth;

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

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            playerDetected = false;
            timer = 0f;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        float verticalDiff = Mathf.Abs(player.position.y - transform.position.y);

        // Nếu player trong range, cùng mức ngang (không quá cao/thấp) và không bị tường che thì phát hiện
        if (distance <= detectRange && verticalDiff <= maxVerticalDiff && CanSeePlayer())
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

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 origin = transform.position;
        Vector2 target = player.position;
        Vector2 dir = target - origin;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir.normalized, detectRange, wallLayer);

        // Nếu ray chạm tường trước player => không thấy player
        return hit.collider == null;
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
        if (playerHealth != null && playerHealth.currentHealth <= 0) return;
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate( bulletPrefab, firePoint.position, Quaternion.identity ); 

        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        if (bulletCol != null && ownerCol != null)
            Physics2D.IgnoreCollision(bulletCol, ownerCol);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.direction = isFacingRight ? Vector2.right : Vector2.left;

        Destroy(bullet, 6f);
    }
}