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
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer mSprite;

    public float m_rotationSpeedFactor = 1.0f;
    public List<GameObject> m_bulletPrefabs = new List<GameObject>();
    public float m_acceleration = 1.0f;
    public float m_rotationDampScale = 0.1f;
    public float m_minWindAccelerationScale = 0.1f;
    public float m_maxSpeed = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<WindController>();
        mWind = GetComponent<Wind>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        mSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 windVelocity = mWind.Velocity;
        float dotWind = Vector2.Dot(windVelocity.normalized, transform.up.normalized);
        dotWind = Mathf.Clamp(dotWind, m_minWindAccelerationScale, 1);

        m_rigidbody.velocity = Vector2.Lerp(m_rigidbody.velocity, transform.up * windVelocity.magnitude/**dotWind*/, m_acceleration);
        m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_maxSpeed);

        // Rotate slowly to facing
        float deltaAngle = Mathf.DeltaAngle(Quaternion.LookRotation(Vector3.forward, -windVelocity.normalized).eulerAngles.z, transform.rotation.eulerAngles.z);
        float torque = deltaAngle - m_rigidbody.angularVelocity;
        torque = torque * m_rotationDampScale;
        m_rigidbody.AddTorque(torque* m_rigidbody.inertia);        
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
                bulletScript.Fire(transform.up);
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
