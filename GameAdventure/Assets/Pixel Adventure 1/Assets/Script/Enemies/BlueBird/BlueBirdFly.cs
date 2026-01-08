using UnityEngine;

public class BlueBirdFly : MonoBehaviour
{
    public float speed = 2f;

    public Rigidbody2D bird;
    public Animator animator;

    void Start()
    {
        
    }

    void Update()
    {
        // Di chuyển
        bird.linearVelocity = new Vector2(-speed, bird.linearVelocity.y);
        
    }
}
