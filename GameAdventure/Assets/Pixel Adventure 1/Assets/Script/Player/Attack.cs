using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public Transform firePoint;
    public Animator animator;
    private PlayerController playerController; // Để private để tránh nhầm lẫn trong Inspector

    void Start()
    {
        // Nếu Animator chưa được kéo vào Inspector, code sẽ tự tìm
        if (animator == null) animator = GetComponent<Animator>();
        
        // Lấy tham chiếu đến PlayerController
        playerController = GetComponent<PlayerController>();

        // Kiểm tra xem có tìm thấy PlayerController không
        if (playerController == null)
        {
            Debug.LogError("Không tìm thấy PlayerController trên cùng GameObject với Attack!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ThrowShuriken();
        }
    }

    void ThrowShuriken()
    {
        if (playerController == null) return;

        // Gọi hàm IsGrounded() từ PlayerController
        bool grounded = playerController.IsGrounded(); 

        if (grounded)
        {
            animator.SetTrigger("Throw");
            Debug.Log("Ném dưới đất");
        }
        else
        {
            animator.SetTrigger("JumpThrow");
            Debug.Log("Ném trên không");
        }

        // --- Logic tạo Shuriken ---
        if (shurikenPrefab != null && firePoint != null)
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
    }
}