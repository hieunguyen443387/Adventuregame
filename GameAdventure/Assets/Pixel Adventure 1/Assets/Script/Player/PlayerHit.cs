using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.Cinemachine; // ✅ Thêm thư viện Cinemachine

public class PlayerHit : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHearts = 3;
    public int currentHearts;
    [Header("Death Effects")]
    public float knockbackForceX = 6f;
    public float knockbackForceY = 10f;
    public float spinSpeed = 720f;
    [Header("References")]
    public Rigidbody2D ninjaFrog;
    private Animator animator;
    private Collider2D playerCollider;
    [Header("Cinemachine Camera")] 
    public CinemachineCamera cinemachineCam; // ✅ Thêm tham chiếu camera
     [Header("Audio Settings")]
	public AudioClip hitSound; // File âm thanh 
    private AudioSource audioSource;
    private PlayerController playerController;
    public bool IsOutOfHearts => currentHearts <= 0;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        currentHearts = maxHearts;

        playerController = GetComponent<PlayerController>(); // ✅ THÊM

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
            ninjaFrog.linearVelocity = Vector2.zero;
            ninjaFrog.gravityScale = 3f;
            ninjaFrog.freezeRotation = false;
            playerCollider.isTrigger = true;

            // 💥 Hất văng
            ninjaFrog.AddForce(new Vector2(knockbackForceX, knockbackForceY), ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap") || other.CompareTag("Enemy") || other.CompareTag("DeadZone") || other.CompareTag("Spike"))
        {
            TakeDamage();
            float hitDir = transform.localScale.x > 0 ? -1f : 1f; 
            playerController.isKnockback = true;
            ninjaFrog.linearVelocity = new Vector2(0f, ninjaFrog.linearVelocity.y);
            ninjaFrog.AddForce(new Vector2(hitDir * knockbackForceX, ninjaFrog.linearVelocity.y),ForceMode2D.Impulse);
            animator.SetTrigger("Hit");
            animator.SetBool("IsJumping", false);
            PlayHitSound();
            GetComponent<PlayerHealth>()?.TakeDamage(1);
            Debug.Log("Player was hit by " + other.tag);
            Invoke(nameof(EndKnockback), 0.25f); 
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

    void EndKnockback()
    {
        playerController.isKnockback = false;
    }

}