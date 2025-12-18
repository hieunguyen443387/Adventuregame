using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.Cinemachine; // ✅ Thêm thư viện Cinemachine

public class PlayerHit : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHearts = 3;
    private int currentHearts;

    [Header("References")]
    public Rigidbody2D ninjaFrog;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private bool isDead = false;
     [Header("Audio Settings")]
	public AudioClip hitSound; // File âm thanh 
    private AudioSource audioSource;

    [Header("Death Knockback")]
    public float knockbackForceX = 5f;
    public float knockbackForceY = 8f;
    public float spinSpeed = 500f;
    public float triggerTime = 0.5f;

    [Header("Cinemachine Camera")]
    public CinemachineCamera cinemachineCam; // ✅ Thêm tham chiếu camera

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("PlayerController cần AudioSource component!");
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        currentHearts = maxHearts;

        // ✅ Tự động tìm Cinemachine camera nếu chưa gán
        if (cinemachineCam == null)
            cinemachineCam = FindAnyObjectByType<CinemachineCamera>();
    }

    void Update()
    {
        if (isDead)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage()
    {
        currentHearts--;
        Debug.Log("Player hit! Hearts left: " + currentHearts);

        if (currentHearts > 0)
        {
            GetComponent<RevivePlayer>()?.RespawnPlayer();
        }
        else
        {
            Debug.Log("Game Over!");

            // ✅ Khi chết hẳn -> ngắt camera follow
            if (cinemachineCam != null)
                cinemachineCam.Follow = null;

            if (playerCollider != null)
                playerCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Trap") || other.CompareTag("Enemy")) && !isDead)
        {
            isDead = true;
            TakeDamage();
            animator.SetTrigger("Hit");
            PlayHitSound();
            GetComponent<PlayerHealth>()?.TakeDamage(1);
            Debug.Log("Player hit a trap!");

            if (currentHearts > 0)
            {
                if (playerCollider != null)
                    StartCoroutine(TemporaryPlayerTrigger());
            }

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 3f;
                rb.freezeRotation = true;

                float direction = (transform.position.x < other.transform.position.x) ? -1 : 1;
                rb.AddForce(new Vector2(direction * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator TemporaryPlayerTrigger()
    {
        playerCollider.isTrigger = true;
        yield return new WaitForSeconds(triggerTime);
        playerCollider.isTrigger = false;
    }

    public void ResetSpin()
    {
        isDead = false;
        transform.rotation = Quaternion.identity;
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }
    private void PlayHitSound()
	{
		if (audioSource != null && hitSound != null)
		{
			// Dùng PlayOneShot để âm thanh nhảy không bị gián đoạn
			audioSource.PlayOneShot(hitSound);
		}
	}
}
