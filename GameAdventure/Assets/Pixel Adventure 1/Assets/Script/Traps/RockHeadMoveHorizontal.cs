using UnityEngine;
using System.Collections;

public class RockHeadMoveHorizontal : MonoBehaviour
{
    public float maxSpeed = 10f;        // tốc độ tối đa
    public float acceleration = 15f;    // tốc độ tăng tốc
    public float pauseTime = 0.5f;     // thời gian khựng lại khi va chạm

    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;         // 1 = phải, -1 = trái
    private float currentSpeed = 0f;   // tốc độ hiện tại
    private bool isPaused = false;     // đang khựng lại hay không

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isPaused) 
        {
            rb.linearVelocity = Vector2.zero; // dừng hoàn toàn
            return;
        }

        // Tăng tốc dần lên tới maxSpeed
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(currentSpeed * direction, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Va chạm trần
            if (collision.contacts[0].normal.x < 0)
            {
                direction = -1;
                anim.SetTrigger("HitRight");
                StartCoroutine(PauseBeforeMove());
            }
            // Va chạm sàn
            else if (collision.contacts[0].normal.x > 0)
            {
                direction = 1;
                anim.SetTrigger("HitLeft");
                StartCoroutine(PauseBeforeMove());
            }
        }
    }

    IEnumerator PauseBeforeMove()
    {
        isPaused = true;
        currentSpeed = 0; // reset lại tốc độ
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;
    }
}