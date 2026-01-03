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

    [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;   // chạy quán tính sau khi húc trượt
    public float pauseAfterHit = 2f;   // đứng yên trước khi đuổi tiếp

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
            animator.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            HandleDirection();   // ⭐ FIX flip loạn ở đây
            Move();
        }
        else
        {
            StopMove();
        }
    }

    // ================= CORE =================

    void HandleDirection()
    {
        // ⭐ CHỈ ĐỔI HƯỚNG KHI PLAYER ĐANG Ở MẶT ĐẤT
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
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            maxSpeed,
            acceleration * Time.fixedDeltaTime
        );

        Rino.linearVelocity = new Vector2(direction * currentSpeed, Rino.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(Rino.linearVelocity.x));
    }

    void StopMove()
    {
        currentSpeed = 0;
        Rino.linearVelocity = new Vector2(0, Rino.linearVelocity.y);
        animator.SetFloat("Speed", 0f);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ================= HIT / INERTIA =================

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (hitRoutine != null)
            StopCoroutine(hitRoutine);

        hitRoutine = StartCoroutine(HitBehaviour());
    }

    IEnumerator HitBehaviour()
    {
        // 🟡 chạy quán tính
        isPaused = false;
        yield return new WaitForSeconds(inertiaTime);

        // 🔴 đứng yên
        isPaused = true;
        Rino.linearVelocity = Vector2.zero;
        animator.SetFloat("Speed", 0f);

        // ⏳ chờ 2s rồi đuổi tiếp
        yield return new WaitForSeconds(pauseAfterHit);

        isPaused = false;
    }
}