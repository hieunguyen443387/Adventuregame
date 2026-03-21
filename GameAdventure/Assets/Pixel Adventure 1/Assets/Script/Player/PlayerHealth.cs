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
    public float spinSpeed = 720f;

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

    void Start()
    {
        dpm = DataPersistanceManager.instance;
        playerCollider = GetComponent<Collider2D>();

        if (cinemachineCam == null)
            cinemachineCam = FindAnyObjectByType<CinemachineCamera>();

        // ===== LOAD DATA =====
        if (dpm != null && dpm.gameData != null)
        {
            // Load máu
            currentHealth = Mathf.Clamp(dpm.gameData.playerHealth, 0, maxHealth);

            // 🔵 Continue → load vị trí đã save
            if (!dpm.gameData.isNewGame)
            {
                transform.position = dpm.gameData.playerPosition;
            }
            // 🟢 New Game → giữ nguyên vị trí spawn trong scene
            else
            {
                dpm.gameData.isNewGame = false; // tránh bị coi là NewGame lần sau
            }
        }
        else
        {
            // Fallback nếu lỗi save system
            currentHealth = maxHealth;
        }

        UpdateHeartsUI();
    }
    void Update()
    {
        if (dpm != null && dpm.gameData != null)
        {
            dpm.gameData.playerPosition = transform.position;
        }
    }

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
            Debug.Log("Player died");
        }
    }

    private void DieEffect()
    {
        Debug.Log("Game Over!");

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

    private void SavePlayerData()
    {
        if (dpm == null || dpm.gameData == null) return;

        // ✅ Cập nhật dữ liệu trước khi save
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

    private void OnDisable()
    {
        // ✅ Save khi đổi scene / tắt object
        SavePlayerData();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
}