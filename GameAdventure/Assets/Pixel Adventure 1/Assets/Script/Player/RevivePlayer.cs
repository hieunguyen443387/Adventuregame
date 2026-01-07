using UnityEngine;
using Unity.Cinemachine;

public class RevivePlayer : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    public CinemachineCamera cinemachineCam;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerHit playerHit;

    private bool isDead = false;
    public bool IsDead => isDead;

    private Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHit = GetComponent<PlayerHit>();

        if (cinemachineCam == null)
            cinemachineCam = FindAnyObjectByType<CinemachineCamera>();

        initialPosition = transform.position;
    }

    // =======================
    // GỌI KHI CHẾT (SPIKE / VỰC)
    // =======================
    public void Die()
    {
        if (isDead) return;

        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.freezeRotation = false;

        if (cinemachineCam != null)
            cinemachineCam.Follow = null;

        animator.SetTrigger("Die");
    }

    // =======================
    // GỌI Ở FRAME CUỐI ANIM DIE
    // =======================
    public void RespawnNow()
    {
        // ❌ HẾT TIM → CHẾT HẲN → KHÔNG RESPAWN
        if (playerHit != null && playerHit.IsOutOfHearts)
        {
            Debug.Log("Out of hearts → Game Over");
            return;
        }

        Vector3 respawnPos = initialPosition;

        if (GameManager.instance != null)
        {
            Vector3 checkpoint = GameManager.instance.GetCheckpoint();
            if (checkpoint != Vector3.zero)
                respawnPos = checkpoint;
        }

        transform.position = respawnPos;
        transform.rotation = Quaternion.identity;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 1f;
        rb.freezeRotation = true;

        animator.Rebind();
        animator.Update(0f);
        animator.Play("Idle");

        if (cinemachineCam != null)
            cinemachineCam.Follow = transform;

        isDead = false;
    }

    // =======================
    // TRIGGER DEADZONE / SPIKE
    // =======================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("DeadZone") || other.CompareTag("Spike"))
        {
            Die();
        }
    }
}