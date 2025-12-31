using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.Cinemachine; // ✅ Thêm thư viện Cinemachine

public class PlayerHit : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHearts = 3;
    private int currentHearts;
    [Header("Death Effects")]
    public float knockbackForceX = 6f;
    public float knockbackForceY = 10f;
    public float spinSpeed = 720f;
    [Header("References")]
    public Rigidbody2D ninjaFrog;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    [Header("Cinemachine Camera")] 
    public CinemachineCamera cinemachineCam; // ✅ Thêm tham chiếu camera
     [Header("Audio Settings")]
	public AudioClip hitSound; // File âm thanh 
    private AudioSource audioSource;

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

    public void TakeDamage()
    {
        currentHearts--;
        Debug.Log("Player hit! Hearts left: " + currentHearts);

        if (currentHearts <= 0)
        {
            Debug.Log("Game Over!");
            // ✅ Khi chết hẳn -> ngắt camera follow 
            if (cinemachineCam != null)
            {
                cinemachineCam.Follow = null;
            }
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 3f;
            rb.freezeRotation = false;
            playerCollider.isTrigger = true;

            // 💥 Hất văng
            rb.AddForce( new Vector2(knockbackForceX, knockbackForceY), ForceMode2D.Impulse );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap") || other.CompareTag("Enemy"))
        {
            TakeDamage();
            animator.SetTrigger("Hit");
            PlayHitSound();
            GetComponent<PlayerHealth>()?.TakeDamage(1);
            Debug.Log("Player hit a trap!");
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
