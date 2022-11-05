using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileSpawningIcon : MonoBehaviour
{
    public float _Lifetime = 1f;

    private void OnEnable()
    {
        Destroy(gameObject, _Lifetime);
    }
}
