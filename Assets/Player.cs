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

    public float m_jinkMagnitude = 5.0f;
    public float m_jinkCooldown = 0.5f;
    public float m_jinkRadialImpulseMin = 2.0f;
    public float m_jinkRadialImpulseMax = 10.0f;
    private bool m_jinkBlocked = false;


    public List<GameObject> m_FiredBullets = new List<GameObject>();
    public int m_StartingBullets = 10;
    public int remainingBullets = 0;
    public int _AmmoOnRefuel = 5;
    public int index = 0;
    public bool _EndGameOnNoAmmo = false;

    public int _MaxPlayerHealth = 3;
    public int _CurrentPlayerHealth = 3;
    public int _HealthLostOnHit = 1;
    public float _DamageImmunityWindow = 1f;
    private bool _IsDamageImmune = false;
    public bool _EndGameOnNoHealth = true;

    public UI_AmmoDisplay m_UIAmmoDisplay = null;
    public UI_HealthDisplay m_UIHealthDisplay = null;


    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<WindController>();
        mWind = GetComponent<Wind>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        mSprite = GetComponent<SpriteRenderer>();

        remainingBullets = m_StartingBullets;
        UpdateBulletIndex();
        _CurrentPlayerHealth = _MaxPlayerHealth;
        UpdateHealthUI();
        _IsDamageImmune = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jink(transform.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Jink(-transform.up);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Jink(-transform.right);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Jink(transform.right);
        }
    }

    private void Jink(Vector3 direction)
    {
        if (m_jinkBlocked)
            return;

        m_rigidbody.AddForce(m_jinkMagnitude * direction, ForceMode2D.Impulse);
        float impulse = Random.Range(m_jinkRadialImpulseMin, m_jinkRadialImpulseMax);
        int positive = Random.Range(0, 2);
        m_rigidbody.AddTorque(positive == 1 ? impulse : -impulse, ForceMode2D.Impulse);

        StartCoroutine(JinkCooldown());
    }

    IEnumerator JinkCooldown()
    {
        m_jinkBlocked = true;
        yield return new WaitForSeconds(m_jinkCooldown);
        m_jinkBlocked = false;
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
        if (remainingBullets <= 0)
            return;

        if (m_bulletPrefabs.Count > 0)
        {
            GameObject bullet = Instantiate(m_bulletPrefabs[index], transform.position, transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript)
            {
                // This is a cheesey way of getting the last fired bullet to message us
                remainingBullets--;                
                bulletScript._Player = this;
                m_FiredBullets.Add(bullet);
                bulletScript.Fire(transform.up);
            }
            else { Destroy(bullet); }

            UpdateBulletIndex();
        }
    }

    public void RemoveFiredBullet(GameObject bullet)
    {
        if (m_FiredBullets.Contains(bullet))
        {
            m_FiredBullets.Remove(bullet);

            if (m_FiredBullets.Count <= 0 && remainingBullets <= 0)
            {
                FinalBulletDestroyed();
            }
        }
    }

    public void FinalBulletDestroyed()
    {
        if (!_EndGameOnNoAmmo)
            return;

        Debug.Log("Final Bullet Destroyed");
        Debug.Break();
    }

    private void UpdateBulletIndex()
    {
        index = Random.Range(0, m_bulletPrefabs.Count);
        UpdateBulletUI();
    }

    private void UpdateBulletUI()
    {
        if (remainingBullets <= 0)
        {
            m_UIAmmoDisplay.NextAmmoType(new Color(0,0,0));

        }
        else
        {
            m_UIAmmoDisplay.NextAmmoType(m_bulletPrefabs[index].GetComponent<SpriteRenderer>().color);
        }

        m_UIAmmoDisplay.NewAmmoCount(remainingBullets);
    }

    public int StealAmmo (int idealAmount)
    {
        Debug.Log("StealAmmo " + idealAmount);
        if (remainingBullets - idealAmount <= 0)
        {
            int tempValue = remainingBullets;
            remainingBullets = 0;
            UpdateBulletUI();
            return tempValue;
        }
        else
        {
            remainingBullets -= idealAmount;
            UpdateBulletUI();
            return idealAmount;
        }
    }

    public void GetBackAmmo (int ammoAmount)
    {
        remainingBullets += ammoAmount;
        UpdateBulletUI();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hostile")
        {
            LoseHealth();
        }
        else if (collision.gameObject.tag == "Target")
        {
            // Do something
        }
        else if (collision.gameObject.tag == "Refueler")
        {
            remainingBullets += _AmmoOnRefuel;
            UpdateBulletUI();
        }
    }

    private void LoseHealth()
    {
        if (_IsDamageImmune) return;

        Debug.Log("Took Damage");
        _CurrentPlayerHealth -= _HealthLostOnHit;
        StartCoroutine(DamageImmunityTimer());
        UpdateHealthUI();

        if (_CurrentPlayerHealth <= 0 && _EndGameOnNoHealth)
        {
            Debug.Log("No Health Remaining");
            Debug.Break();
        }
    }

    IEnumerator DamageImmunityTimer()
    {
        _IsDamageImmune = true;
        yield return new WaitForSeconds(_DamageImmunityWindow);
        _IsDamageImmune = false;
    }

    private void UpdateHealthUI()
    {
        m_UIHealthDisplay.NewHealthValue(_CurrentPlayerHealth);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
