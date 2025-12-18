using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CavePortal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; 
    [SerializeField] private float delayBeforeLoad = 1.5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EnterCaveAndLoad());
        }
    }

    private IEnumerator EnterCaveAndLoad()
    {
        // chạy hiệu ứng fade
        yield return StartCoroutine(ScreenFader.instance.FadeOut(delayBeforeLoad));

        // sau khi fade xong thì load scene mới
        SceneManager.LoadScene("AdventureGame");
    }
}
