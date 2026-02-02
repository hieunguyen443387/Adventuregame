using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    private bool playerDie;
    public bool PlayerDie => playerDie;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // tránh chết nhiều lần

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            playerDie = true;
        }

        UpdateHeartsUI();

        if (currentHealth > 0)
        {
            Debug.Log("Player hit! Current Health: " + currentHealth);
        }
        else
        {
            Debug.Log("Player died, waiting before Game Over...");
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
