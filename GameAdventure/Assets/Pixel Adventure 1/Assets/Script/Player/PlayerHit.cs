using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.Cinemachine; // ✅ Thêm thư viện Cinemachine

public class PlayerHit : MonoBehaviour
{
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



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        playerController = GetComponent<PlayerController>(); // ✅ THÊM

        if (cinemachineCam == null)
            cinemachineCam = FindAnyObjectByType<CinemachineCamera>();
    }

    public void TakeDamage()
    {
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health == null) return;
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