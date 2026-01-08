using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public Vector2 direction = Vector2.left;
    //public GameObject hitEffectPrefab;
    public float effectDuration = 2f; // Thời gian effect tồn tại trước khi bị destroy
    private bool hasHit = false; // Ngăn chặn trigger nhiều lần

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return; // Thoát nếu đã va chạm
        
        Debug.Log("Bullet hit: " + collision.name);
        if (collision.gameObject.CompareTag("Player")){
            hasHit = true;
            
            // if (hitEffectPrefab != null)
            // {
            //     GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            //     Debug.Log("Hit effect instantiated at: " + transform.position);
            //     // Tự động destroy effect sau effectDuration giây
            //     Destroy(effect, effectDuration);
            // }
            // else
            // {
            //     Debug.LogWarning("hitEffectPrefab is not assigned on Bullet.", this);
            // }
            
            Destroy(gameObject);
            Debug.Log("Bullet destroyed after hitting player.");
        }
    }
}
