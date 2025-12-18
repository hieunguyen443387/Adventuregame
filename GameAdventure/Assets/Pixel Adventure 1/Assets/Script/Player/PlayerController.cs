using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("References")]
	public Rigidbody2D ninjaFrog;
	public Animator animator;

	// ⭐ THÊM HEADERS VÀ BIẾN AUDIO MỚI ⭐
	[Header("Audio Settings")]
	public AudioClip jumpSound; // File âm thanh nhảy
	private AudioSource audioSource; // Component phát âm thanh
	// ⭐ KẾT THÚC THÊM BIẾN AUDIO ⭐

	[Header("Movement Settings")]
	public float speed = 5f;
	public float jumpForce = 8f;
	private bool doubleJump;

	[Header("Ground Check")]
	public Transform groundCheck;
	public float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	private bool isGrounded;
	private Vector3 originalScale;
	private int count;

	public float doubleTapTime = 0.1f;   // khoảng thời gian cho phép double tap

	void Start()
	{
		originalScale = transform.localScale;
		// ⭐ LẤY THAM CHIẾU AUDIO SOURCE TẠI ĐÂY ⭐
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			Debug.LogError("PlayerController cần AudioSource component!");
		}
		// ⭐ KẾT THÚC LẤY THAM CHIẾU ⭐
	}

	void Update()
	{
		// ... (Giữ nguyên code kiểm tra Ground và Di chuyển) ...
		if (groundCheck != null)
			isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		// Di chuyển ngang
		float move = Input.GetAxisRaw("Horizontal");
		if (ninjaFrog != null)
		{
			ninjaFrog.linearVelocity = new Vector2(move * speed, ninjaFrog.linearVelocity.y);
			if (animator != null)
				animator.SetFloat("Speed", Mathf.Abs(ninjaFrog.linearVelocity.x));
		}

		// ... (Giữ nguyên code xoay nhân vật) ...
		if (move > 0)
		{
			transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
		}
		else if (move < 0)
		{
			transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
		}

		// Nhảy
		if (isGrounded)
		{
			doubleJump = false;
		}

		// Nhảy hoặc double jump
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (isGrounded)
			{
				// Normal jump
				if (ninjaFrog != null)
					ninjaFrog.linearVelocity = new Vector2(ninjaFrog.linearVelocity.x, jumpForce);

				// ⭐ PHÁT ÂM THANH NHẢY (1) ⭐
				PlayJumpSound();
			}
			else if (!doubleJump)
			{
				// Double jump
				if (ninjaFrog != null)
					ninjaFrog.linearVelocity = new Vector2(ninjaFrog.linearVelocity.x, jumpForce);
				doubleJump = true;

				// ⭐ PHÁT ÂM THANH NHẢY (2) ⭐
				PlayJumpSound();
			}
		}

		// Cập nhật animation nhảy dựa vào trạng thái grounded
		if (animator != null)
			animator.SetBool("IsJumping", !isGrounded);
		if (ninjaFrog != null && animator != null)
			animator.SetFloat("yVelocity", ninjaFrog.linearVelocity.y);
	}

	// ⭐ HÀM PHÁT ÂM THANH MỚI ⭐
	private void PlayJumpSound()
	{
		if (audioSource != null && jumpSound != null)
		{
			// Dùng PlayOneShot để âm thanh nhảy không bị gián đoạn
			audioSource.PlayOneShot(jumpSound);
		}
	}
	// ⭐ KẾT THÚC HÀM PHÁT ÂM THANH MỚI ⭐

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Pickup"))
		{
			Destroy(other.gameObject);
			count++;
			Debug.Log("Picked up item, count = " + count);
		}
	}
}