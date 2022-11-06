using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Target : MonoBehaviour
{
    public EBulletType m_desiredBulletType;
    public float m_lifeTime = 10.0f;
    public GameObject m_indicatorPrefab = null;
    public Canvas m_canvas = null;

    TargetSpawner m_parentSpawner = null;
    SpriteRenderer m_spriteRenderer = null;

    private PositionIndicatorArrow m_indicator;

    // Start is called before the first frame update
    void Start()
    {
        m_parentSpawner = GetComponentInParent<TargetSpawner>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_canvas = FindObjectOfType<Canvas>();

        if (m_canvas != null && m_indicatorPrefab != null)
        {
            GameObject newObject = Instantiate(m_indicatorPrefab, m_canvas.transform);
            m_indicator = newObject.GetComponent<PositionIndicatorArrow>();
            if (m_indicator == null)
            {
                Destroy(newObject);
            }
            else
            {
                m_indicator.m_player = FindObjectOfType<Player>();
                m_indicator.m_target = transform;
            }
        }
    }

    private void OnDestroy()
    {
        if (m_indicator != null)
        {
            Destroy(m_indicator.gameObject);
        }
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
        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
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
        else if (collision.gameObject.tag == "Player")
        {
            // Do something
        }
        else if (collision.gameObject.tag == "Hostile")
        {
            // Do something
        }
    }
}
