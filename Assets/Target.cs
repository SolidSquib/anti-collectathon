using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public EBulletType m_desiredBulletType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
