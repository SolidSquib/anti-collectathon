using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        mRigidbody.AddForce(windVelocity);

        Vector3 targetDirection = mRigidbody.velocity;
        Quaternion deltaRot = Quaternion.LookRotation(Vector3.forward, targetDirection.normalized);
        mSprite.transform.rotation = Quaternion.Lerp(mSprite.transform.rotation, deltaRot, targetDirection.magnitude * m_rotationSpeedFactor * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
