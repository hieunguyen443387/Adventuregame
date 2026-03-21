using UnityEngine;

public class WinGame : MonoBehaviour
{
    [Header("WIN UI")]
    public GameObject winPanel;   // UI chữ WIN
    public GameObject quitButton; // Nút Quit

    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (quitButton != null) quitButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (winPanel != null) winPanel.SetActive(true);
            if (quitButton != null) quitButton.SetActive(true);

            Debug.Log("🏆 YOU WIN!");
        }
    }
}