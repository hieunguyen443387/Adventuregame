using UnityEngine;

public class BossHealth : EnemyHealth
{
    private SnailBehaviour snail; // hoặc script boss của bạn

    protected override void Start()
    {
        base.Start(); // 👈 giữ logic máu gốc
        snail = GetComponent<SnailBehaviour>();
    }

    public override void TakeDamage(int damage)
    {
        // 👉 Nếu đang rolling thì KHÔNG nhận damage
        if (snail != null && snail.IsRolling())
        {
            Debug.Log("Boss đang rolling → miễn nhiễm sát thương");
            return;
        }

        // 👉 Không rolling → damage như Enemy thường
        base.TakeDamage(damage);
    }
}