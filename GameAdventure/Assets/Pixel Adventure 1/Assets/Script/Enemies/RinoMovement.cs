using UnityEngine;
using System.Collections;

public class RinoMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D Rino;
    public Animator animator;
    public RevivePlayer revivePlayer;

    [Header("Chase Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 15f;
    public float pauseTime = 0.5f;
    public float detectRange = 8f;
    public float returnRange = 12f;

    private float currentSpeed = 0f;
    private int direction = 1;
    private bool isPaused = false;
    private bool isFacingRight = true;
    private bool isReturning = false;

    private Vector3 initialPosition;

    void Start()
    {
        Rino = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (player != null && revivePlayer == null)
            revivePlayer = player.GetComponent<RevivePlayer>();

        initialPosition = transform.position;
        animator.SetFloat("Speed", 0f);
    }

    void FixedUpdate()
    {
        // 🧩 Nếu Rino đang pause (đang trong Hit hoặc đang quay về) thì dừng update
        if (isPaused || player == null)
        {
            Rino.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        // 🧩 Nếu player đang chết → Rino không đuổi, chỉ giữ yên
        if (revivePlayer != null && revivePlayer.IsDead)
        {
            Rino.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceFromStart = Vector2.Distance(transform.position, initialPosition);

        if (isReturning)
        {
            ReturnToStart();
            return;
        }

        // 🔹 Nếu player trong vùng phát hiện
        if (distanceToPlayer <= detectRange && distanceFromStart <= returnRange)
        {
            float xDiff = player.position.x - transform.position.x;
            if (Mathf.Abs(xDiff) > 0.1f)
                direction = xDiff > 0 ? 1 : -1;

            if ((direction == -1 && !isFacingRight) || (direction == 1 && isFacingRight))
                Flip();

            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
            Rino.linearVelocity = new Vector2(direction * currentSpeed, Rino.linearVelocity.y);
            animator.SetFloat("Speed", Mathf.Abs(Rino.linearVelocity.x));
        }
        else
        {
            if (distanceFromStart > returnRange)
            {
                isReturning = true;
            }
            else
            {
                Rino.linearVelocity = Vector2.zero;
                currentSpeed = 0;
                animator.SetFloat("Speed", 0f);
            }
        }
    }

    void ReturnToStart()
    {
        float xDiff = initialPosition.x - transform.position.x;

        if (Mathf.Abs(xDiff) > 0.1f)
        {
            direction = xDiff > 0 ? 1 : -1;
            if ((direction == -1 && !isFacingRight) || (direction == 1 && isFacingRight))
                Flip();

            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
            Rino.linearVelocity = new Vector2(direction * currentSpeed, Rino.linearVelocity.y);
            animator.SetFloat("Speed", Mathf.Abs(Rino.linearVelocity.x));
        }
        else
        {
            Rino.linearVelocity = Vector2.zero;
            currentSpeed = 0;
            isReturning = false;
            animator.SetFloat("Speed", 0f);

            if (!isFacingRight) Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Hit");

            isPaused = true;
            currentSpeed = 0;
            Rino.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0f);

            // 🧩 Gọi respawn player
            if (revivePlayer != null && !revivePlayer.IsDead)
                revivePlayer.RespawnPlayer();

            // 🔹 Sau khi húc, tự quay về chỗ cũ
            StartCoroutine(PauseAndReturnToStart());
        }
    }

    IEnumerator PauseAndReturnToStart()
    {
        isPaused = true;
        Rino.linearVelocity = Vector2.zero;
        animator.SetFloat("Speed", 0f);

        yield return new WaitForSeconds(pauseTime);

        // Sau khi pause, quay lại chỗ cũ
        transform.position = initialPosition;
        if (!isFacingRight)
            Flip();

        isPaused = false;
    }
}