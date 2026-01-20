using UnityEngine;
using System.Collections;

public class DuckBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("References")]
    public Transform player;
    public Rigidbody2D Duck;
    public Animator animator;

    [Header("Vision")]
    public LayerMask wallLayer;   // 👈 layer của tường
    public float detectRange = 8f;

    [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;
    public float pauseAfterHit = 2f;
    public float pauseAfterAttack = 2f;
    private bool isFacingRight = true;
    private PlayerHealth playerHealth;
    private Coroutine hitRoutine;
    private bool hitPlayer = false;
    private bool isPaused = false;
    private int direction = 1;

    [Header("Attack Settings")]
    private Vector3 lockedPlayerPos;
    public float jumpForce = 15f;
    private bool hasLockedPosition = false;
    private Coroutine resetAttackRoutine;
    private bool jumpAnticipation;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded;



    void Start()
    {
        Duck = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame

     // 👉 GỌI Ở FRAME CUỐI CỦA ANIM JumpAnticipation
    void Update()
    {   
        animator.SetBool("IsInAir", !isGrounded);
        animator.SetFloat("yVelocity", Duck.linearVelocity.y);
        
        if (isPaused || player == null || hitPlayer)
            return;
        
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange && CanSeePlayer())
        {
            HandleDirection();
            if (isGrounded && !jumpAnticipation)
            {
                lockedPlayerPos = player.position;
                hasLockedPosition = true;
                animator.SetTrigger("JumpAnticipation");
                jumpAnticipation = true;
                Debug.Log("Duck Jump Anticipation triggered" + jumpAnticipation);
            }  
        }
        else
        {
            if (!isGrounded)
                animator.SetBool("IsInAir", true);
        }
    }


    // ================= DETECT PLAYER =================
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

        // 👇 ĐIỀU KIỆN ĐÚNG
        if ((direction == 1 && !isFacingRight) || (direction == -1 && isFacingRight))
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ================= JUMP =================
    // 🔥 ĐƯỢC GỌI TỪ ANIMATION EVENT

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGrounded) return;
        if (collision.collider.CompareTag("Ground"))
        {
            jumpAnticipation = false;   
            Duck.linearVelocity = Vector2.zero;
            isGrounded = true;  
            StartCoroutine(HitBehaviour());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hitRoutine != null)
                StopCoroutine(hitRoutine);
            hitRoutine = StartCoroutine(HitBehaviour());
        }
    }

    public void OnJumpAnticipationEnd()
    {
        Jump();
    }

    void Jump()
    {
        animator.SetBool("IsInAir", true);
        animator.SetFloat("yVelocity", Duck.linearVelocity.y);
        isGrounded = false;
        float distanceX = lockedPlayerPos.x - transform.position.x;
        float gravity = Mathf.Abs(Physics2D.gravity.y * Duck.gravityScale);
        float timeInAir = (2f * jumpForce) / gravity;
        float jumpXSpeed = distanceX / timeInAir;
        if (hasLockedPosition)
        {
            Duck.linearVelocity = new Vector2(jumpXSpeed, jumpForce);
            hasLockedPosition = false; 
        } 
    }

    // ================= COROUNTINE =================

    IEnumerator HitBehaviour()
    {
        yield return new WaitForSeconds(inertiaTime);

        isPaused = true;
        Duck.linearVelocity = Vector2.zero;
        animator.SetFloat("xVelocity", 0f);

        yield return new WaitForSeconds(pauseAfterHit);

        isPaused = false;
    }
}