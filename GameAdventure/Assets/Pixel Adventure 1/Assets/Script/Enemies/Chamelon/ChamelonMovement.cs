using UnityEngine;

public class ChameleonAttack : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Detect Settings")]
    public float detectRange = 3f;
    public float maxVerticalDiff = 1f; // chỉ tấn công khi cùng độ cao

    [Header("Attack Timing")]
    public float attackInterval = 1.5f;

    [Header("Vision")]
    public LayerMask wallLayer;   // layer của tường (che tầm nhìn)

    private float timer;
    private Animator animator;
    private bool isFacingRight = false;
    private bool playerDetected = false;

    private PlayerHealth playerHealth;
    public GameObject attackHitbox;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 🔍 Tự tìm Player
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null) return;

        // ❌ Player chết → không tấn công
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            playerDetected = false;
            timer = 0f;
            animator.SetBool("PlayerDetected", false);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        float verticalDiff = Mathf.Abs(player.position.y - transform.position.y);

        if (distance <= detectRange &&
            verticalDiff <= maxVerticalDiff &&
            CanSeePlayer())
        {
            playerDetected = true;
            animator.SetBool("PlayerDetected", true);

            HandleFlip();

            timer += Time.deltaTime;
            if (timer >= attackInterval)
            {
                animator.SetTrigger("Attack");
                timer = 0f;
            }
        }
        else
        {
            playerDetected = false;
            timer = 0f;
            animator.SetBool("PlayerDetected", false);
        }
    }

    // ======================
    // CHECK LINE OF SIGHT
    // ======================
    bool CanSeePlayer()
    {
        Vector2 origin = transform.position;
        Vector2 target = player.position;
        Vector2 dir = target - origin;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dir.normalized,
            detectRange,
            wallLayer
        );

        // Nếu ray không chạm tường trước player → thấy player
        return hit.collider == null;
    }

    // ======================
    // FLIP THE CHAMELEON
    // ======================
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

    // ======================
    // ATTACK HIT (ANIM EVENT)
    // ======================
    // 👉 GỌI Ở FRAME ĐÁNH TRÚNG
    public void DealDamage()
    {
        if (!playerDetected) return;
        if (playerHealth == null) return;

        playerHealth.TakeDamage(1);
    }

    public void Attack()
    {
        if (!playerDetected) return;
        if (playerHealth != null && playerHealth.currentHealth <= 0) return;

        animator.SetTrigger("Attack");
    }

    // ======================
    // DEBUG
    // ======================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
    public void EnableAttackHitbox()
    {
        attackHitbox.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        attackHitbox.SetActive(false);
    }
}