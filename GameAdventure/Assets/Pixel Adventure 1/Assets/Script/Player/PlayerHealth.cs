using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private RevivePlayer revivePlayer;
    
    [Header("Audio Settings")]
    public AudioClip gameOverSound; // File âm thanh game over
    private AudioSource audioSource; // Component phát âm thanh

    // ⭐ THÊM BIẾN THAM CHIẾU NHẠC NỀN ⭐
    [Header("Music Settings")]
    public AudioSource backgroundMusicSource; // Kéo thả AudioSource của MusicBackground vào đây
    // ⭐ KẾT THÚC THÊM BIẾN ⭐

    [Header("Game Over UI")]
    public GameObject gameOverUI;
    [Header("Play Again UI")]
    public GameObject playAgainUI;
    [Header("Quit Game UI")]
    public GameObject quitGameUI;

    [Header("Delay before Game Over (seconds)")]
    public float gameOverDelay = 0.2f; // Thời gian chờ sau khi chết

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("PlayerController cần AudioSource component!");
        }
        
        // ⭐ KIỂM TRA THAM CHIẾU NHẠC NỀN ⭐
        if (backgroundMusicSource == null)
        {
            Debug.LogError("Chưa gán Background Music Source trong Inspector!");
        }
        // ⭐ KẾT THÚC KIỂM TRA ⭐

        currentHealth = maxHealth;
        revivePlayer = GetComponent<RevivePlayer>();
        UpdateHeartsUI();

        if (gameOverUI != null && playAgainUI != null && quitGameUI != null)
        {
            gameOverUI.SetActive(false);
            playAgainUI.SetActive(false);
            quitGameUI.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHeartsUI();

        if (currentHealth > 0)
        {
            if (revivePlayer != null)
                revivePlayer.RespawnPlayer();
        }
        else
        {
            Debug.Log("Player died, waiting before Game Over...");
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        // Cho player rơi / animation chạy trong 1.5s
        yield return new WaitForSecondsRealtime(gameOverDelay);
        
        // ⭐ DỪNG NHẠC NỀN TẠI ĐÂY ⭐
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop(); 
        }
        // ⭐ KẾT THÚC DỪNG NHẠC NỀN ⭐

        // Hiện UI
        if (gameOverUI != null && playAgainUI != null && quitGameUI != null)
        {
            gameOverUI.SetActive(true);
            playAgainUI.SetActive(true);
            quitGameUI.SetActive(true);
            PlayGameOverSound();
        }

        // Dừng toàn bộ game
        Time.timeScale = 0f;
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
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
}