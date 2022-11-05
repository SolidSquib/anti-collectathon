using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D = null;

    public GameObject _Player = null;
    public float speed = 1f;
    public float maxSpeed = 1f;

    private void FixedUpdate()
    {
        CalculateForce(_Player.transform.position - transform.position);
        ClampVelocity();
    }


    private void CalculateForce(Vector2 direction)
    {
        ApplyDirectionalForce(direction.normalized * speed);
    }

    private void ClampVelocity()
    {
        float currentSpeed = Vector3.Magnitude(rb2D.velocity);

        if (currentSpeed > maxSpeed)
        { 
            Vector3 newVelocity = rb2D.velocity.normalized;
            newVelocity *= maxSpeed;
            rb2D.velocity = newVelocity;
        }
    }

    private void ApplyDirectionalForce(Vector2 force)
    {
        rb2D.AddForce(force);
    }
}
