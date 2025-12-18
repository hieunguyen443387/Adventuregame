using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.left;
    public float spinSpeed = 1000f;

    public Transform sprite; // kéo Sprite con vào Inspector

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        sprite.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
