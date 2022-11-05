using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D = null;

    public GameObject _Player = null;
    public GameObject _DespawnIcon = null;

    public float speed = 1f;
    public float maxSpeed = 1f; 

    public EBulletType m_desiredBulletType;
    public bool _DieOnBulletHit = false;
    public int _MaxBulletHits = 1;
    public bool _DieAfterBulletHitDelay = false;
    public float _DieDelay = 1f;
    public bool _HasLifeDuration = false; // Does the hostile despawn natural even if not hit?
    public float _MinDuration = 3f;
    public float _maxDuration = 10f;
    private int _TotalBulletImpacts = 0;

    private void OnEnable()
    {
        if (_HasLifeDuration)
        {
            float variableLife = Random.Range(_MinDuration, _maxDuration);
            Invoke("Despawn", variableLife);
        }
    }

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

                _TotalBulletImpacts++;

                // Do we want to kill the hostile after x Bullet Hits?
                if (_DieOnBulletHit && _TotalBulletImpacts >= _MaxBulletHits)
                {
                    Destroy(gameObject);
                }
                // Do we want to despawn the hostile after x duration from a Bullet hit instead?
                else if (_DieAfterBulletHitDelay)
                {
                    Destroy(gameObject, _DieDelay);

                }
            }
            else
            {
                // score
            }
        }
    }   

    // If the Hostile has a lifecycle, we despawn it
    private void Despawn()
    {
        GameObject despawnIcon = Instantiate(_DespawnIcon, null);
        despawnIcon.transform.position = transform.position;
        Destroy(gameObject);
    }
}
