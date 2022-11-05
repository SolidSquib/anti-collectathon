using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float m_lifetime = 5.0f;
    public float m_initialVelocity = 5.0f;

    private float m_startTime = 0.0f;
    private Rigidbody2D m_Rigidbody;

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Fire(Vector2 direction)
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_startTime = Time.time;
        m_Rigidbody.velocity = direction * m_initialVelocity;

        Destroy(gameObject, m_lifetime);
    }
}
