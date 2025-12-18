using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.left;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Bullet hit: {collision.gameObject.name} (tag={collision.gameObject.tag})", this);
        // Tự huỷ khi va chạm; nếu bạn muốn chỉ huỷ với target cụ thể,
        // thay điều kiện bằng tag/layer phù hợp (ví dụ: if (collision.CompareTag("Player"))).
        if (collision.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
