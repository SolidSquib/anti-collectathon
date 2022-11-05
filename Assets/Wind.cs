using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private Vector2 mVelocity;
    public float mDecaySpeed = 1.0f;

    public Vector2 Velocity { get => mVelocity; private set => mVelocity = value; }

    public void Add(Vector2 velocity)
    {
        Velocity += velocity;
    }

    public void Update()
    {
        Velocity = Vector2.Lerp(Velocity, Vector2.zero, mDecaySpeed * Time.deltaTime);
    }
}
