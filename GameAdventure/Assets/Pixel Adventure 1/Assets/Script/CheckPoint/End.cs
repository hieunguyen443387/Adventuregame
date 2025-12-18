using UnityEngine;

public class End : MonoBehaviour
{
    private Animator animator;
    [Header("Audio Settings")]
	public AudioClip youWinSound; // File âm thanh game over
    private AudioSource audioSource;
    private bool activated = false;
    [Header("You Win UI")]
    public GameObject youWinUI;
    [Header("Play Again UI")]
    public GameObject playAgainUI;
    [Header("Quit Game UI")]
    public GameObject quitGameUI;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			Debug.LogError("PlayerController cần AudioSource component!");
		}
        if (youWinUI != null)
        {
            youWinUI.SetActive(false);
            playAgainUI.SetActive(false);
            quitGameUI.SetActive(false);
        }
        Time.timeScale = 1f;
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.CompareTag("Player") && youWinUI != null)
        {
            activated = true;
            // Hiện UI
            youWinUI.SetActive(true);
            playAgainUI.SetActive(true);
            quitGameUI.SetActive(true);
            PlayYouWinSound();
            // Dừng toàn bộ game
            Time.timeScale = 0f;
            Debug.Log("Checkpoint saved: " + transform.position);
        }
    }
    private void PlayYouWinSound()
	{
		if (audioSource != null && youWinSound != null)
		{
			// Dùng PlayOneShot để âm thanh nhảy không bị gián đoạn
			audioSource.PlayOneShot(youWinSound);
		}
	}
}
