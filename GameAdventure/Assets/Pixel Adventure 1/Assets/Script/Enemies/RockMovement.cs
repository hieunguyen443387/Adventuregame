using UnityEngine;

public class RockMovement : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 3f;

    public Rigidbody2D rock;
    public Animator animator;

    private Vector2 startPos;
    private int direction = 1; // 1 = phải, -1 = trái

    void Start()
    {
        startPos = rock.position;
    }

    void Update()
    {
        // Di chuyển
        rock.linearVelocity = new Vector2(direction * speed, rock.linearVelocity.y);

        // Update animation
        animator.SetFloat("xVelocity", Mathf.Abs(rock.linearVelocity.x));

        // Giới hạn trái / phải
        if (rock.position.x >= startPos.x + moveDistance)
        {
            direction = -1;
            Flip();
        }
        else if (rock.position.x <= startPos.x - moveDistance)
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