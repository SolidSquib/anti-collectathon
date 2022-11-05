using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float m_lerpSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Camera cam = Camera.main;      
        cam.transform.position = Vector3.LerpUnclamped(cam.transform.position, new Vector3(transform.position.x, transform.position.y, cam.transform.position.z), m_lerpSpeed * Time.deltaTime);
    }
}
