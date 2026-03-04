using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy ID (unique)")]
    public string enemyID;

    [Header("Health")]
    public int maxHealth = 5;
    protected int currentHealth;

    [Header("References")]
    public Animator animator;

    protected virtual void Start()
    {
        LoadEnemyData();
        Debug.Log(gameObject.name + " current health: " + currentHealth);
    }

    public virtual void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " mất " + damage + " máu, còn: " + currentHealth);

        SaveEnemyData(false);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        SaveEnemyData(true);
        animator.SetTrigger("Die");
    }

    // 👉 GỌI TỪ ANIMATION EVENT (frame cuối anim chết)
    public void DestroyEnemy()
    {
        Debug.Log(gameObject.name + " bị destroy");
        gameObject.SetActive(false);
    }

    // ================= SAVE / LOAD =================

    protected void LoadEnemyData()
    {
        var dpm = DataPersistanceManager.instance;
        if (dpm == null || dpm.gameData == null) return;

        var enemies = dpm.gameData.enemies;
        var data = enemies.Find(e => e.enemyID == enemyID);

        if (data == null)
        {
            // Enemy chưa từng bị đụng tới
            currentHealth = maxHealth;
            return;
        }

        if (data.isDead)
        {
            gameObject.SetActive(false);
        }
        else
        {
            currentHealth = data.currentHealth;
            transform.position = data.position;
        }
    }

    protected void SaveEnemyData(bool isDead)
    {
        var dpm = DataPersistanceManager.instance;
        if (dpm == null || dpm.gameData == null) return;

        var enemies = dpm.gameData.enemies;
        var data = enemies.Find(e => e.enemyID == enemyID);

        if (data == null)
        {
            data = new EnemySaveData();
            data.enemyID = enemyID;
            enemies.Add(data);
        }

        data.isDead = isDead;
        data.currentHealth = currentHealth;
        data.position = transform.position;

        dpm.SaveGame(); // 🔥 BẮT BUỘC
    }
}