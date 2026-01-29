using UnityEngine;
using System.Collections;

public class AttackCircleGrow : MonoBehaviour
{
    public float growTime = 0.6f;
    public Vector3 maxScale = new Vector3(4f, 4f, 1f);

    private Vector3 startScale = Vector3.zero;
    private SkrullHealth skrullHealth;

    void Awake()
    {
        skrullHealth = GetComponentInParent<SkrullHealth>();
        transform.localScale = startScale;
    }

    public void StartGrow()
    {
        StopAllCoroutines();
        transform.localScale = startScale;
        gameObject.SetActive(true);
        StartCoroutine(GrowRoutine());
    }

    IEnumerator GrowRoutine()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / growTime;
            transform.localScale = Vector3.Lerp(startScale, maxScale, t);
            yield return null;
        }

        // 🔥 NỔ XONG → báo về boss
        skrullHealth.OnExplosionFinished();

        // 🔥 tự ẩn
        gameObject.SetActive(false);
    }
}