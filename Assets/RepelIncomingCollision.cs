using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RepelIncomingCollision : MonoBehaviour
{
    public float m_repelMagnitude = 10.0f;
    public float m_repelTorque = 3.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int positiveTorque = Random.Range(0, 2);
        if (collision.rigidbody != null)
        {
            collision.rigidbody.AddTorque(positiveTorque == 1 ? m_repelTorque : -m_repelTorque, ForceMode2D.Impulse);

            Vector2 repelDirection = Vector2.zero - collision.GetContact(0).point;
            repelDirection.Normalize();
            collision.rigidbody.AddForce(repelDirection * m_repelMagnitude, ForceMode2D.Impulse);
        }
    }
}
