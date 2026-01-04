using UnityEngine;
using System.Collections;

public class RinoMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D Rino;
    public Animator animator;

    [Header("Chase Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 15f;
    public float detectRange = 8f;

    [Header("Vision")]
    public LayerMask wallLayer;   // 👈 layer của tường

    [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;
    public float pauseAfterHit = 2f;

    private float currentSpeed = 0f;
    private int direction = 1;
    private bool isFacingRight = true;
    private bool isPaused = false;

    private Coroutine hitRoutine;

    void Start()
    {
        Rino = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (isPaused || player == null)
        {
            Rino.linearVelocity = Vector2.zero;
            animator.SetFloat("xVelocity", 0f);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // ❌ Player trong range nhưng bị tường che → KHÔNG đuổi
        if (distance <= detectRange && CanSeePlayer())
        {
            HandleDirection();
            Move();
        }
        else
        {
            StopMove();
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

    // ================= CORE =================

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

    void Move()
    {
        currentSpeed = Mathf.MoveTowards( currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime ); 

        Rino.linearVelocity = new Vector2(direction * currentSpeed, Rino.linearVelocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(Rino.linearVelocity.x));
    }

    void StopMove()
    {
        currentSpeed = 0;
        Rino.linearVelocity = new Vector2(0, Rino.linearVelocity.y);
        animator.SetFloat("xVelocity", 0f);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ================= HIT =================

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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

        isPaused = true;
        Rino.linearVelocity = Vector2.zero;
        animator.SetFloat("xVelocity", 0f);

        yield return new WaitForSeconds(pauseAfterHit);

        isPaused = false;
    }
}