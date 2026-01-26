using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

    [Header("Chase Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 15f;
    protected float currentSpeed = 0f;

    public virtual float Move()
    {
        currentSpeed = Mathf.MoveTowards( currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime ); 
        return currentSpeed;
    }

    public virtual void StopMove()
    {
        currentSpeed = 0;
    }

}