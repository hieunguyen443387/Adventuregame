using UnityEngine;
using System.Collections;

public class AttackCircleGrow : MonoBehaviour
{
    [Header("Grow Settings")]
    public float growTime = 0.6f;
    public float targetRadius = 4f;   // bán kính cuối
    public AnimationCurve growCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 startScale = Vector3.zero;
    private Vector3 targetScale;
    private CircleCollider2D circle;
    private SpriteRenderer sprite;
    private SkrullHealth skrullHealth;

    void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        skrullHealth = GetComponentInParent<SkrullHealth>();

        transform.localScale = startScale;
        circle.radius = 0.1f;
        gameObject.SetActive(false);
    }

    // 🔥 GỌI HÀM NÀY KHI KẾT THÚC ANIM CHARGING
    public void StartGrow()
    {
        StopAllCoroutines();

        gameObject.SetActive(true);
        transform.localScale = startScale;

        targetScale = Vector3.one * targetRadius;
        StartCoroutine(GrowRoutine());
    }

    IEnumerator GrowRoutine()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / growTime;
            float eased = growCurve.Evaluate(t);

            transform.localScale = Vector3.Lerp(startScale, targetScale, eased);
            circle.radius = Mathf.Lerp(0.1f, 0.5f, eased);

            // 👉 hơi fade cho đẹp
            if (sprite != null)
            {
                Color c = sprite.color;
                c.a = Mathf.Lerp(0.9f, 0.6f, eased);
                sprite.color = c;
            }

            yield return null;
        }

        // 💥 Grow xong → boss vào enraged
        if (skrullHealth != null)
            skrullHealth.OnExplosionFinished();
    }
}