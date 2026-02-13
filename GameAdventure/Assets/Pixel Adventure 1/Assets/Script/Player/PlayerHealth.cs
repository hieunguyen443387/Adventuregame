using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    private bool playerDie;
    public bool PlayerDie => playerDie;

    [Header("UI")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private DataPersistanceManager dpm;

    void Start()
    {
        dpm = DataPersistanceManager.instance;

        // ===== LOAD DATA =====
        if (dpm != null && dpm.gameData != null)
        {
            // Load máu
            currentHealth = dpm.gameData.playerHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            // Load vị trí player
            if (dpm.gameData.playerPosition != Vector3.zero)
            {
                transform.position = dpm.gameData.playerPosition;
            }
        }
        else
        {
            // New Game fallback
            currentHealth = maxHealth;
        }

        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SavePlayerHealth();

        UpdateHeartsUI();

        if (currentHealth == 0)
        {
            playerDie = true;
            Debug.Log("Player died");
        }
    }

    private void SavePlayerHealth()
    {
        if (dpm == null || dpm.gameData == null) return;

        dpm.gameData.playerHealth = currentHealth;
        dpm.gameData.playerPosition = transform.position;
        dpm.gameData.lastSceneName = SceneManager.GetActiveScene().name;

        dpm.SaveGame();
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }

    private void OnApplicationQuit()
    {
        SavePlayerHealth();
    }
}