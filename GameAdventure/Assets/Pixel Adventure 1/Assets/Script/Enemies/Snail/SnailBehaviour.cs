using UnityEngine;
using System.Collections;

public class SnailBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("References")]
    public Transform player;
    public Rigidbody2D Snail;
    public Animator animator;
    //public GameObject attackHitbox;

    [Header("Chase Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 15f;
    public float detectRange = 8f;
    public float spinSpeed = 720f;

    [Header("Vision")]
    public LayerMask wallLayer;   // 👈 layer của tường

    [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;
    public float pauseAfterHit = 2f;

    private float currentSpeed = 0f;
    private int direction = 1;
    private bool isFacingRight = true;
    private bool isPaused = false;
    private PlayerHealth playerHealth;
    //private bool isAttacking = false;
    private bool inShell;
    private bool rolling;
    private bool hitPlayer = false ;


    private Coroutine hitRoutine;
    private Collider2D snailCollider;
    void Start()
    {
        Snail = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        snailCollider = GetComponent<Collider2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        // ⚠ Luôn tắt hitbox khi start
        // if (attackHitbox != null)
        //     attackHitbox.SetActive(false);
    }

     // 👉 GỌI Ở FRAME CUỐI CỦA ANIM InShell
    public void StartRolling()
    {
        rolling = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPaused || player == null || hitPlayer)
        {
            Snail.linearVelocity = Vector2.zero;
            animator.SetBool("Rolling", false);
            return;
        }
        
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectRange && CanSeePlayer())
        {
            HandleDirection();
            if (!inShell)
            {
                animator.SetTrigger("InShell");
                inShell = true;
            }
            if (rolling)
            {
                Move();
            }
        }
        else 
        {
            snailCollider.enabled = true;
            if (inShell || hitPlayer) 
            {
                rolling = false;
                animator.SetTrigger("OutShell");
                inShell = false;
                StopMove();
            }
        }
    }

    // ================= VISION =================


    bool CanSeePlayer()
    {
        Vector2 origin = transform.position;
        Vector2 target = player.position;
        Vector2 dir = target - origin;

        RaycastHit2D hit = Physics2D.Raycast( origin, dir.normalized, detectRange, wallLayer );

        // Nếu ray đụng tường trước → không thấy player
        return hit.collider == null;
    }

    // ================= FLIP =================

    void HandleDirection()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null && !pc.isGrounded) return;

        float xDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(xDiff) < 0.1f) return;

        direction = xDiff > 0 ? 1 : -1;

        if ((direction == -1 && !isFacingRight) || (direction == 1 && isFacingRight))
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ================= MOVE =================
    void Move()
    {
        currentSpeed = Mathf.MoveTowards( currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime ); 
        Snail.linearVelocity = new Vector2(direction * currentSpeed, Snail.linearVelocity.y);
        animator.SetBool("Rolling", true);
    }

    void StopMove()
    {
        currentSpeed = 0;
        //rolling = false; 
        Snail.linearVelocity = new Vector2(0, Snail.linearVelocity.y);
        animator.SetBool("Rolling", false);
    }

    // ================= HIT =================

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rolling = false; 
            hitPlayer = true; 
            Debug.Log("hitPlayer = true");
            animator.SetTrigger("OutShell");
            
            if (hitRoutine != null)
                StopCoroutine(hitRoutine);
            hitRoutine = StartCoroutine(HitBehaviour());
        }

        if (collision.CompareTag("Wall"))
            animator.SetTrigger("HitWall");
    }

    IEnumerator HitBehaviour()
    {
        yield return new WaitForSeconds(inertiaTime);
        hitPlayer = false; 
        isPaused = true;
        inShell = false;      // 🔥 BẮT BUỘC RESET
        rolling = false;      // 🔥 AN TOÀN
        Snail.linearVelocity = Vector2.zero;
        snailCollider.enabled = true;
        //animator.SetTrigger("OutShell");

        yield return new WaitForSeconds(pauseAfterHit);

        isPaused = false;
    }

    // 👉 Frame đánh trúng
    // public void EnableAttackHitbox()
    // {
    //     if (!CanSeePlayer()) return;
    //     if (playerHealth.currentHealth <= 0) return;

    //     attackHitbox.SetActive(true);
    // }

    // // 👉 Frame kết thúc đánh
    // public void DisableAttackHitbox()
    // {
    //     attackHitbox.SetActive(false);
    //     isAttacking = false;
    // }
}