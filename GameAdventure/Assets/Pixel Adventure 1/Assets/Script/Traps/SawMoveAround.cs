using UnityEngine;

public class SawMoveAround : MonoBehaviour
{
    public Transform center;    // tảng đá
    public float speed = 50f;   // tốc độ quay
    public float radius = 2f;   // khoảng cách từ tâm (viền tảng đá)

    private void Start()
    {
        if (center != null)
        {
            // Đặt lưỡi cưa ở đúng vị trí bán kính
            transform.position = center.position + Vector3.right * radius;
        }
    }

    private void Update()
    {
        if (center != null)
        {
            // Quay quanh tảng đá theo trục Z
            transform.RotateAround(center.position, Vector3.forward, speed * Time.deltaTime);
        }
    }
}
