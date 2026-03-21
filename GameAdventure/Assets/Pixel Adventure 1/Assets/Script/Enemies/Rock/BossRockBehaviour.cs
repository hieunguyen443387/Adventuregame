using UnityEngine;
using System.Collections;

public class BossRockBehaviour : EnemyBase
{
    [Header("References")]
    public Transform player;
    private Rigidbody2D rb;
    private DetectPlayer detect;
    private RockHealth rockHealth;

    private int direction;
    private bool hitPlayer = false;

    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        detect = GetComponent<DetectPlayer>();
        rockHealth = GetComponent<RockHealth>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        if (isPaused || player == null || hitPlayer || playerHealth.PlayerDie)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detect.detectRange && detect.CanSeePlayer())
        {
            detect.HandleDirection();
            direction = detect.Direction;

            Move();
        }
        else
        {
            StopMove();
        }
    }

    // ================= MOVE =================

    public override float Move()
    {
        base.Move();

        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(currentSpeed));

        return currentSpeed;
    }

    public override void StopMove()
    {
        base.StopMove();
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    // ================= HIT =================

    protected override void HandlePlayerHit()
    {
        hitPlayer = true;

        base.HandlePlayerHit();
    }

    protected override void OnAfterInertia()
    {
        hitPlayer = false;
        isPaused = true;

        rb.linearVelocity = Vector2.zero;
    }

    protected override void OnHitFinished()
    {
        hitPlayer = false;
    }
}