using UnityEngine;
using System.Collections;

public class SnailBehaviour : EnemyBase
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    private DetectPlayer detect;
    private Collider2D col;

    [Header("State")]
    private int direction;
    private bool inShell;
    private bool rolling;
    private bool hitPlayer;
    private bool isRecovering;

    private float lockedPlayerX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        detect = GetComponent<DetectPlayer>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    // 🔔 gọi bằng Animation Event cuối anim InShell
    public void StartRolling()
    {
        rolling = true;
    }

    public bool IsRolling()
    {
        return rolling;
    }

    void FixedUpdate()
    {
        if (player == null || isPaused || hitPlayer || isRecovering)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("Rolling", false);
            return;
        }

        if (!detect.CanSeePlayer())
            return;

        direction = detect.Direction;
        detect.HandleDirection();

        // ====== CHƯA VÀO SHELL ======
        if (!inShell && !isRecovering)
        {
            EnterShell();
            return;
        }

        // ====== ĐANG LĂN ======
        if (rolling)
        {
            MoveRolling();

            bool passedPlayer = (direction == 1 && transform.position.x > lockedPlayerX) || (direction == -1 && transform.position.x < lockedPlayerX);

            if (passedPlayer)
            {
                StartCoroutine(RecoverAfterRoll());
            }
        }
    }

    void EnterShell()
    {
        inShell = true;
        rolling = false;

        lockedPlayerX = player.position.x;
        animator.SetTrigger("InShell");
    }

    void MoveRolling()
    {
        Move();
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);
        animator.SetBool("Rolling", true);
    }

    IEnumerator RecoverAfterRoll()
    {
        isRecovering = true;

        // ⛔ dừng quán tính
        rolling = false;
        StopMove();
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("Rolling", false);
        animator.SetTrigger("OutShell");

        inShell = false;

        // ⏸ đứng lại
        yield return new WaitForSeconds(0.6f);

        isRecovering = false;
    }

    // ========= HIT PLAYER =========
    protected override void HandlePlayerHit()
    {
        hitPlayer = true;
        rolling = false;
        inShell = false;

        StopMove();
        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("OutShell");
        base.HandlePlayerHit();
    }

    protected override void OnAfterInertia()
    {
        hitPlayer = false;
    }
}