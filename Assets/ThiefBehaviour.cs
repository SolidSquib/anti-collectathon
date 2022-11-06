using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefBehaviour : MonoBehaviour
{
    //approach Player and take item
    // run away from player

    [SerializeField] private Rigidbody2D rb2D = null;
    [SerializeField] private SpriteRenderer _SpriteRenderer = null;

    public GameObject _Player = null;
    public HostileSpawner hostileSpawner = null;

    public float chasingspeed = 1f;
    public float chasingmaxSpeed = 1f;
    private bool isChasingPlayer = true;

    public float runningawayspeed = 1f;
    public float runningawaymaxSpeed = 1f;

    public int _IdealAmountToSteal = 3;
    public int _CurrentAmmo = 0;

    public float maxDistanceFromPlayer = 10f; // If it gets too far from the Player when fleeing, stop it moving

    public Color chasingColour;
    public Color runningawayColour;

    private void OnEnable()
    {
        isChasingPlayer = true;
        _CurrentAmmo = 0;
        ChangeSpriteColour();
    }

    private void FixedUpdate()
    {
        if (isChasingPlayer)
        {
            CalculateForce(_Player.transform.position - transform.position);
        }
        else if (Vector2.Distance(_Player.transform.position, transform.position) < maxDistanceFromPlayer)
        { 
            CalculateForce(transform.position - _Player.transform.position);        
        }
        ClampVelocity();
    }

    private void CalculateForce(Vector2 direction)
    {
        ApplyDirectionalForce(direction.normalized * (isChasingPlayer ? chasingspeed : runningawayspeed));
    }

    private void ClampVelocity()
    {
        float currentSpeed = Vector3.Magnitude(rb2D.velocity);

        if (currentSpeed > (isChasingPlayer ? chasingmaxSpeed : runningawaymaxSpeed))
        {
            Vector3 newVelocity = rb2D.velocity.normalized;
            newVelocity *= (isChasingPlayer ? chasingmaxSpeed : runningawaymaxSpeed);
            rb2D.velocity = newVelocity;
        }
    }

    private void ApplyDirectionalForce(Vector2 force)
    {
        rb2D.AddForce(force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isChasingPlayer)
            {
                StealAmmoFromPlayer();

                if (_CurrentAmmo > 0) // this means we actually stole something from the Player
                {
                    isChasingPlayer = false;
                    ChangeSpriteColour();
                }
            }
            else
            {
                ReturnAmmoToPlayer();
                hostileSpawner.ThiefDestroyed(gameObject);
                Destroy(gameObject);
            }           
        }
        else if (collision.gameObject.tag == "Target")
        {
        }
    }

    private void StealAmmoFromPlayer()
    {
        int stolenAmmo = _Player.GetComponent<Player>().StealAmmo(_IdealAmountToSteal);
        _CurrentAmmo += stolenAmmo;
    }

    public void ReturnAmmoToPlayer()
    {
        _Player.GetComponent<Player>().GetBackAmmo(_CurrentAmmo);

    }

    private void ChangeSpriteColour()
    {
        _SpriteRenderer.color = isChasingPlayer ? chasingColour : runningawayColour;
    }
}
