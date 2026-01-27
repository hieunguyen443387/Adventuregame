using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public LayerMask wallLayer;

    [Header("Vision")]
    public float detectRange = 8f;

    [Header("Flip")]
    public bool isFacingRight = true;
    public int Direction => isFacingRight ? 1 : -1;

    void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    // ================= VISION =================
    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 origin = transform.position;
        Vector2 target = player.position;

        float distance = Vector2.Distance(origin, target);
        return distance <= detectRange;
    }

    // ================= FLIP =================
    public void HandleDirection()
    {
        if (player == null) return;

        float xDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(xDiff) < 0.1f) return;

        int direction = xDiff > 0 ? 1 : -1;

        if ((direction == 1 && !isFacingRight) || (direction == -1 && isFacingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
