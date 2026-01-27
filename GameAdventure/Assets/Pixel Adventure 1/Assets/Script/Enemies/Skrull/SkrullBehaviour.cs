using UnityEngine;
using System.Collections;

public class SkrullBehaviour : EnemyBase
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D Skrull;
    private DetectPlayer detect;  
    private EnemyBase enemyBase; 

    private int direction;
    private PlayerHealth playerHealth;
    private bool hitPlayer = false ;
    private float lockedPlayerPos;
    private bool hasLockedPosition = false;

    private Coroutine hitRoutine;
    private Collider2D skrullCollider;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        Skrull = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        skrullCollider = GetComponent<Collider2D>();
        detect = GetComponent<DetectPlayer>();
        enemyBase = GetComponent<EnemyBase>();

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
    void FixedUpdate()
    {
        if (isPaused || player == null || hitPlayer)
        {
            Skrull.linearVelocity = Vector2.zero;
            return;
        }
        
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detect.detectRange && detect.CanSeePlayer())
        {
            direction = detect.Direction;
            detect.HandleDirection();
            lockedPlayerPos = player.position.x;
            hasLockedPosition = true;
            
            Move();
            bool passedPlayer = (direction == 1 && transform.position.x > lockedPlayerPos) || (direction == -1 && transform.position.x < lockedPlayerPos);
            if (passedPlayer)
            {
                StopMove();
            }
        }
        else 
        {
            skrullCollider.enabled = true;
            if (hitPlayer) 
            {
                StopMove();
            }
        }
    }

    // ================= MOVE =================
    public virtual float Move()
    {
        base.Move(); 
        Skrull.linearVelocity = new Vector2(direction * currentSpeed, Skrull.linearVelocity.y);
        return currentSpeed;
    }

    public virtual void StopMove()
    {
        base.StopMove();
        Skrull.linearVelocity = new Vector2(0, Skrull.linearVelocity.y);
    }

    // ================= HIT =================

    protected override void HandlePlayerHit()
    {
        hitPlayer = true;

        Debug.Log("Skrull hitPlayer = true");

        base.HandlePlayerHit(); // 🔥 vẫn dùng logic chung
    }

    protected override void OnAfterInertia()
    {
        Debug.Log("Skrull OnAfterInertia");
        hitPlayer = false; 
        isPaused = true;
        Skrull.linearVelocity = Vector2.zero;
        skrullCollider.enabled = true;
    }

    protected override void OnHitFinished()
    {
        Debug.Log("Skrull Hit Finished");
    }

}
