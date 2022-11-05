using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wind))]
public class WindController : MonoBehaviour
{
    private Wind mWind;
    public float mMouseSensitivity = 5.0f;
    private Vector2 touchStart = Vector2.zero;
    private float touchStartTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        mWind = GetComponent<Wind>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            touchStartTime = Time.time;
            //Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //mWind.Add(mouseDelta * mMouseSensitivity);
        }
        else if (Input.GetMouseButtonUp(0) && touchStartTime > 0)
        {
            Vector2 touchEnd = Input.mousePosition;
            Vector2 touchDelta = touchEnd - touchStart;

            Vector2 impulse = touchDelta / (Time.time - touchStartTime);
            mWind.Add(impulse);

            touchStartTime = 0;
        }
    }
}
