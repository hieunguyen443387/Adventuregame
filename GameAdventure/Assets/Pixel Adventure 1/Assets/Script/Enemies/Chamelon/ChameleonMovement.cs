using UnityEngine;

public class ChameleonMovement : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 3f;

    public Rigidbody2D chameleon;
    public Animator animator;

    private Vector2 startPos;
    private int direction = 1; // 1 = phải, -1 = trái
    private ChameleonAttack chameleonAttack;

    void Start()
    {
        startPos = chameleon.position;
        chameleonAttack = GetComponent<ChameleonAttack>();
    }

    void Update()
    {
        if (chameleonAttack != null)
        {
            // Nếu phát hiện player thì dừng di chuyển
            animator.SetFloat("xVelocity", 0);
            return;
        }
        // Di chuyển
        chameleon.linearVelocity = new Vector2(direction * speed, chameleon.linearVelocity.y);

        // Update animation
        animator.SetFloat("xVelocity", Mathf.Abs(chameleon.linearVelocity.x));

        // Giới hạn trái / phải
        if (chameleon.position.x >= startPos.x + moveDistance)
        {
            direction = -1;
            Flip();
        }
        else if (chameleon.position.x <= startPos.x - moveDistance)
        {
            direction = 1;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}