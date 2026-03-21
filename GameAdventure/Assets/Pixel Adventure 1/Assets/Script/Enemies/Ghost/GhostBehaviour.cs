using UnityEngine;
using System.Collections;

public class GhostBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("References")]
    public Transform player;
    public Rigidbody2D Ghost;
    public Animator animator;
    public SpriteRenderer ghostSprite;
    public Collider2D ghostCollider;
    public Collider2D boxHit;
    private DetectPlayer detect;  

    [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;
    public float disappearCooldown = 1.2f; // thời gian nghỉ giữa Disappear → Appear
    public float pauseAfterHit = 2f;
    private bool isTransitioning = false; // đang Disappear → chặn logic
    private bool isFacingRight = true;
    private PlayerHealth playerHealth;
    private Coroutine hitRoutine;
    private bool hitPlayer = false;
    private bool isPaused = false;
    private bool appear = true;
    private int direction = 1;
    [Header("Teleport Settings")]
    public float teleportYOffset = 1.0f; // con ma xuất hiện cao hơn player bao nhiêu
    private Vector3 lockedPlayerPos;
    private bool hasLockedPosition = false;
    private bool canAttack = true;
    private Coroutine resetAttackRoutine;
    
    void Start()
    {
        Ghost = GetComponent<Rigidbody2D>();
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
    void FixedUpdate()
    {
        if (isPaused || player == null || hitPlayer || isTransitioning)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detect.detectRange && detect.CanSeePlayer())
        {
            detect.HandleDirection();
            if (appear && canAttack)
            {
                lockedPlayerPos = player.position + Vector3.up * teleportYOffset;
                hasLockedPosition = true;
                animator.ResetTrigger("Appear");
                animator.ResetTrigger("Disappear");
                animator.SetTrigger("Disappear");
                appear = false;
                isTransitioning = true;
                canAttack = false;
            }
        }
    }


    // ================= APPEAR =================

    public void Disappear()
    {
        ghostSprite.enabled = false;
        ghostCollider.enabled = false;
        boxHit.enabled = false;
        StartCoroutine(DisappearCooldown());
    }

    public void Appear()
    {
        ghostSprite.enabled = true;
        ghostCollider.enabled = true;
        boxHit.enabled = true;

        TeleportToPlayer();

        if (resetAttackRoutine != null)
            StopCoroutine(resetAttackRoutine);

        resetAttackRoutine = StartCoroutine(ResetAttack());
    }

    void TeleportToPlayer()
    {
        if (player == null) return;
        if (hasLockedPosition)
        {
            transform.position = lockedPlayerPos;
            hasLockedPosition = false; 
        }
    }

    // ================= HIT =================

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hitPlayer = true; 
            if (hitRoutine != null)
                StopCoroutine(hitRoutine);
            hitRoutine = StartCoroutine(HitBehaviour());
        }
    }

    // ================= COROUNTINE =================
    IEnumerator DisappearCooldown()
    {
        yield return new WaitForSeconds(disappearCooldown);
        animator.SetTrigger("Appear");
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(disappearCooldown);
        isTransitioning = false;
        canAttack = true;
        appear = true;
    }

    IEnumerator HitBehaviour()
    {
        yield return new WaitForSeconds(inertiaTime);
        hitPlayer = false; 
        isPaused = true;

        yield return new WaitForSeconds(pauseAfterHit);
        isPaused = false;
    }
}