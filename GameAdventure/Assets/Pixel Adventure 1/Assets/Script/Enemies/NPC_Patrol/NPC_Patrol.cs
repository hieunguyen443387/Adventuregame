using UnityEngine;

public class NPC_Patrol : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 3f;

    public Rigidbody2D npc;
    public Animator animator;

    private Vector2 startPos;
    private int direction = 1; // 1 = phải, -1 = trái

    void Start()
    {
        startPos = npc.position;
    }

    void Update()
    {
        // Di chuyển
        npc.linearVelocity = new Vector2(direction * speed, npc.linearVelocity.y);

        // Update animation
        animator.SetFloat("xVelocity", Mathf.Abs(npc.linearVelocity.x));
        // Giới hạn trái / phải
        if (npc.position.x >= startPos.x + moveDistance)
        {
            direction = -1;
            Flip();
        }
        else if (npc.position.x <= startPos.x - moveDistance)
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