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
        if (currentHealth <= 5)
        {
            isEnraged = true;
            animator.SetTrigger("Charging");
        }

        base.TakeDamage(damage);
    }

    // 🔥 GỌI TỪ ANIMATION EVENT CUỐI CHARGING
    public void OnChargingEnd()
    {
        isCharging = false;
        animator.ResetTrigger("Charging");

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
        if (attackCircleGrow != null)
            attackCircleGrow.SetActive(false);

        if (attackCircleCollider != null)
            attackCircleCollider.SetActive(true);

        if (attackHitBox != null)
            attackHitBox.SetActive(true);

        animator.SetBool("isEnraged", isEnraged);
    }

    protected override void Die()
    {
        base.Die();

        isEnraged = false;
        animator.SetBool("isEnraged", false);

        if (attackCircleGrow != null)
            attackCircleGrow.SetActive(false);

        if (attackCircleCollider != null)
            attackCircleCollider.SetActive(false);
    }
}
