using UnityEngine;
using System.Collections;

public class SnailBehaviour : EnemyMove
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("References")]
    public Transform player;
    public Rigidbody2D Snail;
    public Animator animator;
    private DetectPlayer detect;  
    private EnemyMove move; 

    [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;
    public float pauseAfterHit = 2f;

    private int direction;
    private bool isPaused = false;
    private PlayerHealth playerHealth;
    private bool inShell;
    private bool rolling;
    private bool hitPlayer = false ;
    private float lockedPlayerPos;
    private bool hasLockedPosition = false;

    private Coroutine hitRoutine;
    private Collider2D snailCollider;
    void Start()
    {
        Snail = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        snailCollider = GetComponent<Collider2D>();
        detect = GetComponent<DetectPlayer>();
        move = GetComponent<EnemyMove>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
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
        if (distance <= detect.detectRange && detect.CanSeePlayer())
        {
            direction = detect.Direction;
            detect.HandleDirection();
            if (!inShell)
            {
                lockedPlayerPos = player.position.x;
                hasLockedPosition = true;
                animator.SetTrigger("InShell");
                inShell = true;
            }
            if (rolling)
            {
                Move();
                bool passedPlayer =
                    (direction == 1 && transform.position.x > lockedPlayerPos) ||
                    (direction == -1 && transform.position.x < lockedPlayerPos);

                if (passedPlayer)
                {
                    rolling = false;
                    StopMove();
                    animator.SetTrigger("OutShell");
                    inShell = false;
                }
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

    // ================= MOVE =================
    public virtual float Move()
    {
        base.Move(); 
        Snail.linearVelocity = new Vector2(direction * currentSpeed, Snail.linearVelocity.y);
        animator.SetBool("Rolling", true);
        return currentSpeed;
    }

    public virtual void StopMove()
    {
        base.StopMove();
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

    public bool IsRolling()
    {
        return rolling;
    }

}