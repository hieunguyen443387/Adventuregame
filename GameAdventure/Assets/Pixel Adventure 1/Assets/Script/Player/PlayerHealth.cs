using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        // 🔥 LẤY MÁU TỪ SAVE (Continue)
        if (DataPersistanceManager.instance != null &&
            DataPersistanceManager.instance.gameData != null)
        {
            currentHealth = DataPersistanceManager.instance.gameData.playerHealth;
        }
        else
        {
            // fallback (New Game)
            currentHealth = maxHealth;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 🔥 GHI VÀO SAVE DATA
        if (DataPersistanceManager.instance != null)
        {
            DataPersistanceManager.instance.gameData.playerHealth = currentHealth;
        }

        UpdateHeartsUI();

        if (currentHealth == 0)
        {
            playerDie = true;
            Debug.Log("Player died");
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }
}