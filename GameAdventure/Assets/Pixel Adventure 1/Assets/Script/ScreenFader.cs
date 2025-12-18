using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;  // Singleton để gọi từ script khác
    private Image fadeImage;

    private void Awake()
    {
        if (instance == null) instance = this;
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeOut(float duration)
    {
        Color color = fadeImage.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t / duration); // tăng alpha từ 0 → 1
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1;
        fadeImage.color = color;
    }
}
