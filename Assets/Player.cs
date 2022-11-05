using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WindController))]
[RequireComponent(typeof(Wind))]
public class Player : MonoBehaviour
{
    private WindController mController;
    private Wind mWind;

    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<WindController>();
        mWind = GetComponent<Wind>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
