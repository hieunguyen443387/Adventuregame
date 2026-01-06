using UnityEngine;

public class ChameleonAttackHitbox : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHit>()?.TakeDamage();
        }
    }
}
