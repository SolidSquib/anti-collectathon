using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(WindController))]
[RequireComponent(typeof(Wind))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private WindController mController;
    private Wind mWind;
    private Rigidbody2D mRigidbody;
    private SpriteRenderer mSprite;

    public float m_rotationSpeedFactor = 1.0f;
    public List<GameObject> m_bulletPrefabs = new List<GameObject>();
    public float m_acceleration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<WindController>();
        mWind = GetComponent<Wind>();
        mRigidbody = GetComponent<Rigidbody2D>();
        mSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 windVelocity = mWind.Velocity;
        mRigidbody.velocity = Vector2.Lerp(mRigidbody.velocity, windVelocity, m_acceleration);

        Vector3 targetDirection = mRigidbody.velocity;
        Quaternion deltaRot = Quaternion.LookRotation(Vector3.forward, targetDirection.normalized);
        mSprite.transform.rotation = Quaternion.Lerp(mSprite.transform.rotation, deltaRot, targetDirection.magnitude * m_rotationSpeedFactor * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (m_bulletPrefabs.Count > 0)
        {
            int index = Random.Range(0, m_bulletPrefabs.Count);
            GameObject bullet = Instantiate(m_bulletPrefabs[index], transform.position, transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript)
            {
                bulletScript.Fire(-transform.up);
            }
            else { Destroy(bullet); }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
