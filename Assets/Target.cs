using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Target : MonoBehaviour
{
    public EBulletType m_desiredBulletType;
    public float m_lifeTime = 10.0f;

    TargetSpawner m_parentSpawner = null;
    SpriteRenderer m_spriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        m_parentSpawner = GetComponentInParent<TargetSpawner>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_spriteRenderer.isVisible)
        {
            m_lifeTime -= Time.deltaTime;
            if (m_lifeTime <= 0)
            {
                DestroyTarget();
            }
        }
    }

    public void DestroyTarget()
    {
        if (m_parentSpawner != null)
        {
            m_parentSpawner.RemoveTarget(this);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision detected");
        Bullet bullet = collision.collider.GetComponent<Bullet>();
        if (bullet)
        {
            Debug.Log("bullet collision detected");
            if (bullet.m_bulletType == m_desiredBulletType)
            {
                // more score
            }
            else
            {
                // score
            }

            DestroyTarget();
        }
    }
}
