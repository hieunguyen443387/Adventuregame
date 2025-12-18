using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Cinemachine;

public class RevivePlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D playerCollider;

    [Header("Respawn Settings")]
    public float respawnDelay = 1.5f;
    private bool isDead = false;

    private Vector3 initialPosition;
    // public read-only property để các script khác kiểm tra trạng thái
    public bool IsDead => isDead;


    [Header("Cinemachine Camera")]
    public CinemachineCamera cinemachineCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        if (cinemachineCam == null)
            cinemachineCam = FindAnyObjectByType<CinemachineCamera>();

        initialPosition = transform.position;
    }

    public void RespawnPlayer()
    {
        if (!isDead)
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;

            // 🛑 Ngắt Follow của Cinemachine Camera khi nhân vật chết
            if (cinemachineCam != null)
                cinemachineCam.Follow = null; 

            if (playerCollider != null)
                playerCollider.enabled = false;

            Invoke(nameof(DoRespawn), respawnDelay);
        }
    }

    private void DoRespawn()
    {
        Vector3 checkpointPos = GameManager.instance.GetCheckpoint();
        if (checkpointPos == Vector3.zero)
            checkpointPos = initialPosition;

        transform.position = checkpointPos;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1f;
        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;

        if (playerCollider != null)
            playerCollider.enabled = true;

        PlayerHit hit = GetComponent<PlayerHit>();
        if (hit != null)
            hit.ResetSpin();

        animator.Play("Idle");

        // ✅ Gán lại follow cho camera khi hồi sinh
        if (cinemachineCam != null)
            cinemachineCam.Follow = transform;

        isDead = false;
    }
}