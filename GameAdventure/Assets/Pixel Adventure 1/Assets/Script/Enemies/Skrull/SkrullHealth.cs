using UnityEngine;

public class SkrullHealth : EnemyHealth
{
    private SkrullBehaviour skrullBehaviour;

    private bool isCharging = false;
    private bool isEnraged = false;

    [Header("Attack Circle")]
    public GameObject attackCircleGrow;       // 👈 vòng đỏ phồng
    public GameObject attackCircleCollider;   // 👈 vòng sát thương
    public GameObject attackHitBox;   
    private AttackCircleGrow growScript;
    private bool hasExplodedOnDeath = false;
    private bool hasCharged = false;
    private bool isDead = false;
    public bool IsCharging => isCharging;

    protected override void Start()
    {
        base.Start();

        skrullBehaviour = GetComponent<SkrullBehaviour>();

        if (attackCircleGrow != null)
        {
            growScript = attackCircleGrow.GetComponent<AttackCircleGrow>();
            attackCircleGrow.SetActive(false);
        }

        if (attackCircleCollider != null)
        {
            attackCircleCollider.SetActive(false);
        }

        if (attackHitBox != null)
        {
            attackHitBox.SetActive(true);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // trừ máu trước

        if (currentHealth <= 5 && !hasCharged)
        {
            hasCharged = true;      // 🔒 KHÓA
            isCharging = true;
            isEnraged = true;
            animator.SetTrigger("Charging");
        }
    }

    // 🔥 GỌI TỪ ANIMATION EVENT CUỐI CHARGING
    public void OnChargingEnd()
    {
        isCharging = false;
        animator.ResetTrigger("Charging");

        OnExplosionStart();
    }

    public void OnExplosionStart()
    {
        // bật vòng đỏ phồng
        if (attackCircleGrow != null)
        {
            attackCircleGrow.SetActive(true);
            growScript.StartGrow();
        }
    }

    // 🔥 GỌI KHI VÒNG ĐỎ PHỒNG XONG
    public void OnExplosionFinished()
    {
        if (isDead) return;  

        if (attackCircleCollider != null)
            attackCircleCollider.SetActive(true);

        if (attackHitBox != null)
            attackHitBox.SetActive(false);

        animator.SetBool("isEnraged", isEnraged);
    }

    protected override void Die()
    {
        if (hasExplodedOnDeath) return;   
        hasExplodedOnDeath = true;
        isDead = true;
        isEnraged = false;
        animator.SetBool("isEnraged", false);
        OnExplosionStart();
        if (attackCircleCollider != null)
            attackCircleCollider.SetActive(false);
        base.Die();
    }

    public void HideEnemy()
    {
        // tắt animation
        animator.enabled = false;
        // tắt renderer
        GetComponent<SpriteRenderer>().enabled = false; 
        // tắt collider
        GetComponent<Collider2D>().enabled = false;

        Debug.Log("Enemy hidden");
    }
}