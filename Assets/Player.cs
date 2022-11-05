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

    public List<GameObject> m_FiredBullets = new List<GameObject>();
    public int m_TotalBullets = 10;
    public int remainingBullets = 0;
    public int index = 0;

    public UI_AmmoDisplay m_UIAmmoDisplay = null;
    
    
    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<WindController>();
        mWind = GetComponent<Wind>();
        mRigidbody = GetComponent<Rigidbody2D>();
        mSprite = GetComponent<SpriteRenderer>();

        remainingBullets = m_TotalBullets;
        UpdateBulletIndex();
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
                bulletScript.Fire(-transform.up);
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hostile")
        {
            // Do something
        }
        else if (collision.gameObject.tag == "Target")
        {
            // Do something
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
