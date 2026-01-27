using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {

    [Header("Chase Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 15f;
    protected float currentSpeed = 0f;

     [Header("After Hit Settings")]
    public float inertiaTime = 0.4f;
    public float pauseAfterHit = 2f;
    protected bool isPaused;
    protected Animator animator;
    private Coroutine hitRoutine;

    public virtual float Move()
    {
        currentSpeed = Mathf.MoveTowards( currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime ); 
        return currentSpeed;
    }

    public virtual void StopMove()
    {
        currentSpeed = 0;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HandlePlayerHit();
        }
        else if (collision.CompareTag("Wall"))
        {
            HandleWallHit();
        }
    }

    protected virtual void HandlePlayerHit()
    {
        if (hitRoutine != null)
            StopCoroutine(hitRoutine);

        hitRoutine = StartCoroutine(HitBehaviour());
    }

    protected virtual void HandleWallHit()   // ✅ BẮT BUỘC virtual
    {
        animator.SetTrigger("HitWall");
    }

    public virtual IEnumerator HitBehaviour()
    {
        // ⏱ Chờ inertia
        yield return new WaitForSeconds(inertiaTime);

        // 🔹 hook cho enemy con
        OnAfterInertia();

        isPaused = true;

        // ⏱ Chờ pause
        yield return new WaitForSeconds(pauseAfterHit);

        isPaused = false;

        // 🔹 hook sau cùng
        OnHitFinished();
    }

    // 👇 enemy con chỉ override mấy hàm này
    protected virtual void OnAfterInertia() { }
    protected virtual void OnHitFinished() { }

}