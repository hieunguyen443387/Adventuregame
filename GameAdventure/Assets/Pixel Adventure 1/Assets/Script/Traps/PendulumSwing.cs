using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    [Header("Cấu hình lắc lư")]
    public float speed = 2f;       // tốc độ lắc
    public float angle = 45f;      // góc tối đa (độ)
    private float startAngle;

    void Start()
    {
        // Lưu lại góc ban đầu
        startAngle = transform.localEulerAngles.z;
    }

    void Update()
    {
        // Tính toán góc lắc qua lại bằng sin
        float zRotation = angle * Mathf.Sin(Time.time * speed);
        transform.localRotation = Quaternion.Euler(0, 0, startAngle + zRotation);
    }
}
