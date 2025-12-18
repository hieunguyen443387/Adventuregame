using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private bool activated = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;
            animator.SetTrigger("isCheckPoint"); // chạy animation mở cờ
            GameManager.instance.SetCheckpoint(transform.position); // lưu vị trí checkpoint
            Debug.Log("Checkpoint saved: " + transform.position);
        }
    }
}