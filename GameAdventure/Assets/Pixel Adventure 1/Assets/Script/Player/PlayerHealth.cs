using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    private bool playerDie;
    public bool PlayerDie => playerDie;
    public bool IsOutOfHearts => currentHealth <= 0;

    [Header("Death Effects")]
    public float knockbackForceX = 6f;
    public float knockbackForceY = 10f;

    [Header("Cinemachine Camera")] 
    public CinemachineCamera cinemachineCam;

    [Header("References")]
    public Rigidbody2D ninjaFrog;
    private Collider2D playerCollider;

    [Header("UI")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private DataPersistanceManager dpm;

    // ================= START =================
    void Start()
    {
        dpm = DataPersistanceManager.instance;
        playerCollider = GetComponent<Collider2D>();

        if (cinemachineCam == null)
            cinemachineCam = FindAnyObjectByType<CinemachineCamera>();

        LoadPlayerData();
        UpdateHeartsUI();
    }

    // ================= UPDATE =================
    void Update()
    {
        // Autosave vị trí realtime
        if (dpm != null && dpm.gameData != null)
        {
            dpm.gameData.playerPosition = transform.position;
        }
    }

    // ================= LOAD =================
    private void LoadPlayerData()
    {
        if (dpm == null || dpm.gameData == null)
        {
            currentHealth = maxHealth;
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;

        // ⭐ NEW GAME
        if (dpm.gameData.isNewGame)
        {
            currentHealth = maxHealth;
            dpm.gameData.playerHealth = currentHealth;

            // spawn mặc định theo scene
            dpm.gameData.playerPosition = transform.position;
            dpm.gameData.lastSceneName = currentScene;

            dpm.gameData.isNewGame = false;
            return;
        }

        // 🔵 CONTINUE GAME
        currentHealth = Mathf.Clamp(dpm.gameData.playerHealth, 0, maxHealth);

        // ⭐ CHỈ load vị trí nếu cùng scene
        if (dpm.gameData.lastSceneName == currentScene)
        {
            transform.position = dpm.gameData.playerPosition;
        }
        // 🟢 Scene mới → spawn mặc định
        else
        {
            dpm.gameData.playerPosition = transform.position;
            dpm.gameData.lastSceneName = currentScene;
        }
    }

    // ================= DAMAGE =================
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SavePlayerData();
        UpdateHeartsUI();

        if (currentHealth == 0)
        {
            playerDie = true;
            DieEffect();
        }
    }

    private void DieEffect()
    {
        if (cinemachineCam != null)
            cinemachineCam.Follow = null;

        ninjaFrog.linearVelocity = Vector2.zero;
        ninjaFrog.gravityScale = 3f;
        ninjaFrog.freezeRotation = false;
        playerCollider.isTrigger = true;

        ninjaFrog.AddForce(
            new Vector2(knockbackForceX, knockbackForceY),
            ForceMode2D.Impulse
        );
    }

    // ================= SAVE =================
    private void SavePlayerData()
    {
        if (dpm == null || dpm.gameData == null) return;

        dpm.gameData.playerHealth = currentHealth;
        dpm.gameData.playerPosition = transform.position;
        dpm.gameData.lastSceneName = SceneManager.GetActiveScene().name;

        dpm.SaveGame();
    }

    // ================= UI =================
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }

    // ================= AUTO SAVE =================
    private void OnDisable()
    {
        SavePlayerData();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
}