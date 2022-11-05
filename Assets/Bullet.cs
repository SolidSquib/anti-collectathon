using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBulletType { Green, Red }

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public float m_lifetime = 5.0f;
    public float m_initialVelocity = 5.0f;
    public EBulletType m_bulletType;

    private float m_startTime = 0.0f;
    private Rigidbody2D m_Rigidbody;
    private Collider2D m_collider;

    private Player _Player = null;

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Fire (Vector2 direction)
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_startTime = Time.time;
        m_Rigidbody.velocity = direction * m_initialVelocity;

        Invoke("DestroyBullet", m_lifetime);
        //Destroy(gameObject, m_lifetime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            DestroyBullet();

        }
        else if (collision.gameObject.tag == "Hostile")
        {
            DestroyBullet();

        }
    }

    private void DestroyBullet()
    {
        if (_Player != null)
        { 
            _Player.FinalBulletDestroyed();
        }
            
        Destroy(gameObject);
    }
}

// Bullet
// Target
// Player
// Hostile