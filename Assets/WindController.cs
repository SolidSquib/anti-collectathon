using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wind))]
public class WindController : MonoBehaviour
{
    private Wind mWind;
    public float mMouseSensitivity = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        mWind = GetComponent<Wind>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {            
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));        

            mWind.Velocity += mouseDelta * mMouseSensitivity;
            Debug.Log($"WindSpeed is  {mWind.Velocity.x}, {mWind.Velocity.y}");
        }
    }
}
