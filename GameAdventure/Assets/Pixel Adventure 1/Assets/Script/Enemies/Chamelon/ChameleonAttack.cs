using UnityEngine;

public class ChameleonAttack : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject attackHitbox;
    public Rigidbody2D chameleon;

    [Header("Detect Settings")]
    public float detectRange = 3f;
    public float maxVerticalDiff = 1f;

    [Header("Attack Timing")]
    public float attackInterval = 1.5f;

    [Header("Vision")]
    public LayerMask wallLayer;
    [Header("Movement")]
    public float speed = 2f;
    public float moveDistance = 3f;
    private Vector2 startPos;

    private Animator animator;
    private float timer;
    private bool isFacingRight = false;
    private bool playerDetected = false;
    private int direction = 1; // 1 = phải, -1 = trái

    private PlayerHealth playerHealth;
    private bool isAttacking = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = chameleon.position;

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        // ⚠ Luôn tắt hitbox khi start
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    void Update()
    {
        Move();
        if (player == null || playerHealth == null) return;
        float distance = Vector2.Distance(transform.position, player.position);
        float verticalDiff = Mathf.Abs(player.position.y - transform.position.y);

        // ❌ Player chết → STOP TẤT CẢ
        if (distance <= detectRange && verticalDiff <= maxVerticalDiff && CanSeePlayer())
        {
            playerDetected = true;
            HandleFlip();

            if (!isAttacking)
            {
                timer += Time.deltaTime;
                if (timer >= attackInterval)
                {
                    isAttacking = true;
                    animator.SetTrigger("Attack");
                    timer = 0f;
                }
            }
        }
        else
        {
            ResetAttack();
        }

    }

    void Move()
    {
        if (playerDetected)
        {
            // Nếu phát hiện player thì dừng di chuyển
            animator.SetFloat("xVelocity", 0);
            return;
        }
        // Di chuyển
        chameleon.linearVelocity = new Vector2(direction * speed, chameleon.linearVelocity.y);

        // Update animation
        animator.SetFloat("xVelocity", Mathf.Abs(chameleon.linearVelocity.x));

        // Giới hạn trái / phải
        if (chameleon.position.x >= startPos.x + moveDistance)
        {
            direction = -1;
            if (!isFacingRight) Flip();
        }
        else if (chameleon.position.x <= startPos.x - moveDistance)
        {
            direction = 1;
            if (isFacingRight) Flip();
        }
    }

    void ResetAttack()
    {
        playerDetected = false;
        timer = 0f;

        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    bool CanSeePlayer()
    {
        Vector2 origin = transform.position;
        Vector2 dir = player.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast( origin, dir.normalized, detectRange, wallLayer );

        return hit.collider == null;
    }

    void HandleFlip()
    {
        float xDiff = player.position.x - transform.position.x;

        if (xDiff > 0 && isFacingRight)
            Flip();
        else if (xDiff < 0 && !isFacingRight)
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
    // ANIMATION EVENTS
    // ======================

    // 👉 Frame đánh trúng
    public void EnableAttackHitbox()
    {
        if (!playerDetected) return;
        if (playerHealth.currentHealth <= 0) return;

        attackHitbox.SetActive(true);
    }

    // 👉 Frame kết thúc đánh
    public void DisableAttackHitbox()
    {
        attackHitbox.SetActive(false);
        isAttacking = false;
    }

    // ======================
    // DEBUG
    // ======================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}