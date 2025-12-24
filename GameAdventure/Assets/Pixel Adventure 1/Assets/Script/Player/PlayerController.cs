using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("References")]
	public Rigidbody2D ninjaFrog;
	public Animator animator;

	// ⭐ THÊM HEADERS VÀ BIẾN AUDIO MỚI ⭐
	[Header("Audio Settings")]
	public AudioClip jumpSound; // FILE ÂM THANH NHẢY
	private AudioSource audioSource; // COMPONENT PHÁT ÂM THANH
	// ⭐ KẾT THÚC THÊM BIẾN AUDIO ⭐

	[Header("Movement Settings")]
	public float speed = 5f;
	public float jumpForce = 8f;
	public bool doubleJump;

	[Header("Ground Check")]
	public Transform groundCheck;
	public float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public bool isGrounded;
	private Vector3 originalScale;
	private int count;

	public float doubleTapTime = 0.1f;   // KHOẢNG THỜI GIAN CHO PHÉP DOUBLE TAP
	[Header("Attack Settings")]
	public GameObject shurikenPrefab;
    public Transform firePoint;

	void Start()
	{
		originalScale = transform.localScale;
		//  LẤY THAM CHIẾU AUDIO SOURCE TẠI ĐÂY 
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			Debug.LogError("PlayerController cần AudioSource component!");
		}
		// ⭐ KẾT THÚC LẤY THAM CHIẾU ⭐
	}

	void Update()
	{
		// GIỮ NGUYÊN CODE KIỂM TRA GROUND VÀ DI CHUYỂN
		if (groundCheck != null)
			isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		// DI CHUYỂN NGANG
		float move = Input.GetAxisRaw("Horizontal");
		if (ninjaFrog != null)
		{
			ninjaFrog.linearVelocity = new Vector2(move * speed, ninjaFrog.linearVelocity.y);
			if (animator != null)
				animator.SetFloat("Speed", Mathf.Abs(ninjaFrog.linearVelocity.x));
		}

		// ... (GIỮ NGUYÊN CODE XOAY NHÂN VẬT) ...
		if (move > 0)
		{
			transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
		}
		else if (move < 0)
		{
			transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
		}

		// JUMP LOGIC
		if (isGrounded)
		{
			doubleJump = false;
		}

		// JUMP OR DOUBLE JUMP
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (isGrounded)
			{
				// NORMAL JUMP
				if (ninjaFrog != null){
					ninjaFrog.linearVelocity = new Vector2(ninjaFrog.linearVelocity.x, jumpForce);
					Debug.Log("Jump");
				}

				if(Input.GetKeyDown(KeyCode.F)){
					ThrowShuriken();
					animator.SetTrigger("JumpThrow");
					Debug.Log("Ném trên không");
				}

				// ⭐ PHÁT ÂM THANH NHẢY (1) ⭐
				PlayJumpSound();
			}
			else if (!doubleJump)
			{
				// DOUBLE JUMP
				if (ninjaFrog != null)
					ninjaFrog.linearVelocity = new Vector2(ninjaFrog.linearVelocity.x, jumpForce);
				doubleJump = true;

				// ⭐ PHÁT ÂM THANH NHẢY (2) ⭐
				PlayJumpSound();
			}
		}

		// CAP NHẬT ANIMATION CHO JUMPING
		if (animator != null)
			animator.SetBool("IsJumping", !isGrounded);
		if (ninjaFrog != null && animator != null)
			animator.SetFloat("yVelocity", ninjaFrog.linearVelocity.y);

		if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {
            ThrowShuriken();
			animator.SetTrigger("Throw");
            Debug.Log("Ném dưới đất");
        }
	}

	// ⭐ HÀM PHÁT ÂM THANH MỚI ⭐
	private void PlayJumpSound()
	{
		if (audioSource != null && jumpSound != null)
		{
			// DÙNG PLAYONESHOT ĐỂ PHÁT ÂM THANH NHẢY
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
	public bool IsGrounded()
	{
		return isGrounded;
	}

	void ThrowShuriken()
    {

        // if (isGrounded)
        // {
        //     animator.SetTrigger("Throw");
        //     Debug.Log("Ném dưới đất");
        // }
        // else
        // {
		// 	animator.ResetTrigger("Throw");
        //     animator.SetTrigger("JumpThrow");
        //     Debug.Log("Ném trên không");
		// 	Debug.Log("IsGrounded = " + isGrounded);
        // }

        // --- LOGIC TẠO SHURIKEN ---
        if (shurikenPrefab != null && firePoint != null)
        {
            GameObject shuriken = Instantiate(shurikenPrefab, firePoint.position, Quaternion.identity);

            // BỎ QUA VA CHẠM GIỮA NGƯỜI CHƠI VÀ SHURIKEN
            Collider2D shurikenCol = shuriken.GetComponent<Collider2D>();
            Collider2D ownerCol = GetComponent<Collider2D>();
            if (shurikenCol && ownerCol)
                Physics2D.IgnoreCollision(shurikenCol, ownerCol);

            // XÁC ĐỊNH HƯỚNG DỰA VÀO LOCALSCALE CỦA PLAYER
            Shuriken shurikenScript = shuriken.GetComponent<Shuriken>();
            if (shurikenScript != null)
            {
                // NINJA FROG ĐIỀU KHIỂN HƯỚNG BẰNG LOCAL SCALE TRÊN TRỤC X
                float dir = transform.localScale.x > 0 ? 1f : -1f;
                shurikenScript.direction = new Vector2(dir, 0);
            }

            Destroy(shuriken, 6f);
        }
    }

}