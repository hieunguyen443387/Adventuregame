using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.left;
    public float spinSpeed = 1000f;
    public int damage = 1;   // 💥 damage của shuriken

    public Transform sprite;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        sprite.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // kiểm tra enemy có EnemyHealth không
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // 👈 TRUYỀN DAMAGE VÀO ĐÂY
            Destroy(gameObject);     // shuriken biến mất sau khi trúng
        }
    }
}