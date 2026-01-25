using UnityEngine;
using System.Collections;

public class DuckBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("References")]
    public Transform player;
    public Rigidbody2D Duck;
    public Animator animator;
    private DetectPlayer detect;  

    [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;
    public float pauseAfterHit = 2f;
    public float pauseAfterAttack = 2f;
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
        detect = GetComponent<DetectPlayer>();

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
        
        if (distance <= detect.detectRange && detect.CanSeePlayer())
        {
            detect.HandleDirection();
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