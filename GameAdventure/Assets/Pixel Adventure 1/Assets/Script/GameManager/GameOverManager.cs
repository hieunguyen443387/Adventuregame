using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip gameOverSound; // File âm thanh game over
    private AudioSource audioSource; // Component phát âm thanh

    // ⭐ THÊM BIẾN THAM CHIẾU NHẠC NỀN ⭐
    [Header("Music Settings")]
    public AudioClip backgroundMusicSource; // Kéo thả AudioSource của MusicBackground vào đây
    // ⭐ KẾT THÚC THÊM BIẾN ⭐

    [Header("Game Over UI")]
    public GameObject gameOverUI;
    [Header("Play Again UI")]
    public GameObject playAgainUI;
    [Header("Quit Game UI")]
    public GameObject quitGameUI;
    [Header("Game Over Panel")]
    public GameObject gameOverPanel;

    [Header("Delay before Game Over (seconds)")]
    public float gameOverDelay = 0.2f; // Thời gian chờ sau khi chết
    [Header("References")]
    private PlayerHealth playerHealth;
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
        // if (audioSource == null)
        // {
        //     Debug.LogError("PlayerController cần AudioSource component!");
        // }
        
        // ⭐ KIỂM TRA THAM CHIẾU NHẠC NỀN ⭐
        if (backgroundMusicSource == null)
        {
            Debug.LogError("Chưa gán Background Music Source trong Inspector!");
        }
        // ⭐ KẾT THÚC KIỂM TRA ⭐

        if (gameOverUI != null && playAgainUI != null && quitGameUI != null)
        {
            gameOverUI.SetActive(false);
            playAgainUI.SetActive(false);
            quitGameUI.SetActive(false);
            gameOverPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (playerHealth == null) return;

        // 🔥 ĐỐI CHIẾU HEART Ở ĐÂY
        if (playerHealth.currentHealth <= 0)
        {
            StartCoroutine(GameOverSequence());
            enabled = false; // tránh gọi nhiều lần
        }
    }

    private IEnumerator GameOverSequence()
    {
        // Cho player rơi / animation chạy trong 1.5s
        yield return new WaitForSecondsRealtime(gameOverDelay);

        // Hiện UI
        if (gameOverUI != null && playAgainUI != null && quitGameUI != null)
        {
            gameOverUI.SetActive(true);
            playAgainUI.SetActive(true);
            quitGameUI.SetActive(true);
            gameOverPanel.SetActive(true);
            PlayGameOverSound();
        }

    }

    private void PlayGameOverSound()
    {
        if (audioSource != null && gameOverSound != null)
        {
            // Dùng PlayOneShot để âm thanh nhảy không bị gián đoạn
            audioSource.PlayOneShot(gameOverSound);
        }
    }

    // Hàm gọi khi bấm nút "Play Again"
    public void PlayAgain()
    {
        // Lấy tên scene hiện tại và load lại
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    // Hàm gọi khi bấm "Quit"
    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Game Quit!"); // chỉ hiện khi test trong Editor
    }
}