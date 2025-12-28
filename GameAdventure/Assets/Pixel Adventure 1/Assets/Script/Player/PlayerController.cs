using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D ninjaFrog;
    public Animator animator;

    [Header("Audio Settings")]
    public AudioClip jumpSound;
    private AudioSource audioSource;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 15f;
    // Multiplier applied to horizontal control when airborne (0-1)
    public float airControlMultiplier = 10f;
    private bool doubleJump;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Attack Settings")]
    public GameObject shurikenPrefab;
    public Transform firePoint;
    [Header("Wall Slide Settings")]
    //public float wallSlideSpeed = 2f;
    private bool isOnWall;
    private Vector3 originalScale;
    [Header("Wall Jumping Settings")]
    public float wallJumpForce = 8f;
    public float wallJumpHorizontalForce = 100f;
    private bool isWallJumping;

    void Start()
    {
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckGround();
        Move();
        Attack();
        Jump();
        UpdateAnimator();
        HandleWallSlide();
    }

    // ================= GROUND =================
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);

        if (isGrounded)
            doubleJump = false;
    }

    // ================= MOVE =================
    void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // 🟢 ĐANG Ở MẶT ĐẤT → set cứng
        if (isGrounded)
        {
            ninjaFrog.linearVelocity = new Vector2(move * speed, ninjaFrog.linearVelocity.y );
        }
        // 🔵 TRÊN KHÔNG → cộng lực ngang
        else
        {
            ninjaFrog.linearVelocity = new Vector2(move * 10f, ninjaFrog.linearVelocity.y );
        }

        animator.SetFloat("xVelocity", Mathf.Abs(ninjaFrog.linearVelocity.x));

        if (move > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (move < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    // ================= ATTACK =================
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isOnWall){
                animator.SetTrigger("Throw");
                animator.SetFloat("xVelocity", Mathf.Abs(ninjaFrog.linearVelocity.x));
                Debug.Log("Throw");
                ThrowShuriken();
            }
        }
    }

    // ================= JUMP =================
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 1️⃣ Nhảy từ đất
            if (isGrounded)
            {
                ninjaFrog.linearVelocity = new Vector2( ninjaFrog.linearVelocity.x, jumpForce );

                doubleJump = false;
                Debug.Log("Single Jump");
            }
            // 2️⃣ Double jump (KHÔNG được ở tường)
            else if (!doubleJump && !isOnWall)
            {
                ninjaFrog.linearVelocity = new Vector2( ninjaFrog.linearVelocity.x, jumpForce ); 
                doubleJump = true;
                Debug.Log("Double Jump");
            }

            // 3️⃣ Wall jump
            else if (isOnWall)
            {
                isWallJumping = true;

                // Đẩy người chơi ra khỏi tường
                float wallDir = transform.localScale.x > 0 ? -1f : 1f; // Giả sử tường ở bên phải nếu localScale.x > 0
                ninjaFrog.linearVelocity = new Vector2(wallDir * wallJumpHorizontalForce, wallJumpForce);
                Debug.Log("Wall Jump");
            }
            PlayJumpSound();
        }
    }

    // ================= ANIMATOR =================
    void UpdateAnimator()
    {
        // WallHold chỉ phụ thuộc vào việc đang ở tường (và không ở đất)
        bool wallState = isOnWall && !isGrounded;

        animator.SetBool("WallHold", wallState);

        // Jump chỉ khi ở trên không và KHÔNG ở tường
        animator.SetBool("IsJumping", !isGrounded && !wallState);
    }

    // ================= SHURIKEN =================
    void ThrowShuriken()
    {
        GameObject shuriken = Instantiate(shurikenPrefab, firePoint.position, Quaternion.identity);
		// Bỏ qua va chạm giữa người chơi và shuriken
		Collider2D shurikenCol = shuriken.GetComponent<Collider2D>();
		Collider2D ownerCol = GetComponent<Collider2D>();
		if (shurikenCol && ownerCol)
			Physics2D.IgnoreCollision(shurikenCol, ownerCol);

		// Xác định hướng dựa vào localScale của Player
		Shuriken shurikenScript = shuriken.GetComponent<Shuriken>();
		if (shurikenScript != null)
		{
			// ninjaFrog thường xoay theo transform của cha, ta lấy hướng từ đây
			float dir = transform.localScale.x > 0 ? 1f : -1f;
			shurikenScript.direction = new Vector2(dir, 0);
		}

		Destroy(shuriken, 6f);
    }

    // ================= AUDIO =================
    void PlayJumpSound()
    {
        if (audioSource && jumpSound)
            audioSource.PlayOneShot(jumpSound);
    }


    // ================= ON WALL =================
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            isOnWall = true;
            // animator.SetBool("WallHold", true);
            Debug.Log("Va chạm Wall");
            
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            isOnWall = false;
            //animator.SetBool("WallHold", false);
            Debug.Log("Rời khỏi Wall");
        }
    }

    // ================= WALL SLIDE =================
    void HandleWallSlide()
    {
        if (isOnWall && !isGrounded && ninjaFrog.linearVelocity.y < 0)
        {
            ninjaFrog.linearVelocity = new Vector2(ninjaFrog.linearVelocity.x, 0f);
        }
    }

}