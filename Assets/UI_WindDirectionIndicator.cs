using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WindDirectionIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _WindArrow;

    [SerializeField] private Wind _Wind;

    [SerializeField] private Vector2 testWindValue; // Use for temporary values to set in editor
    [SerializeField] private Vector2 windValueNormalized; // debug
    [SerializeField] private float minScaleClamp = 0.1f;
    [SerializeField] private float maxScaleClamp = 0.5f;

    [SerializeField] private float angle; // debug
    [SerializeField] private float windValueMagnitude; // debug
    [SerializeField] private float magnitudeClamp = 1f;
    [SerializeField] private Vector3 _CurrentArrowDirection; // debug   

    [SerializeField] private float _ScaleLerpSpeed = 1f;
    [SerializeField] private float _RotateLerpSpeed = 1f;

    public void Update()
    {
        SetArrowRotation(_Wind.Velocity);
        SetArrowScale(_Wind.Velocity);

        // Testing
        //_CurrentArrowDirection = _WindArrow.transform.localRotation.eulerAngles;
    }

    private void SetArrowRotation (Vector2 windValue)
    {
        if (windValue != Vector2.zero)
        {
            windValueNormalized = windValue.normalized;
            angle = Mathf.Atan2(windValueNormalized.y, windValueNormalized.x) * Mathf.Rad2Deg;
            _WindArrow.transform.localRotation = Quaternion.Slerp(_WindArrow.transform.rotation, Quaternion.Euler(0.0f, 0.0f, angle), _RotateLerpSpeed * Time.deltaTime);
        }        
    }

    private void SetArrowScale (Vector2 windValue)
    {
        float scaleRatio = windValue.magnitude / _Wind.m_maxSpeed;
        float targetScale = scaleRatio * maxScaleClamp;

        //Vector3 newScale = Vector3.ClampMagnitude(windValue, magnitudeClamp);
        //windValueMagnitude = newScale.magnitude;;     
        //windValueMagnitude = Mathf.Clamp(windValueMagnitude / 2f, minScaleClamp, maxScaleClamp);
        
       // Vector3 desiredScale = new Vector3 (windValueMagnitude, windValueMagnitude, _WindArrow.transform.localScale.y);
        Vector3 lerpedScale = Vector3.Slerp(_WindArrow.transform.localScale, new Vector3(targetScale, targetScale, _WindArrow.transform.localScale.y), _ScaleLerpSpeed * Time.deltaTime);
        _WindArrow.transform.localScale = lerpedScale;

        //_WindArrow.transform.localScale = new Vector3 (windValueMagnitude, windValueMagnitude, _WindArrow.transform.localScale.y);
    }
}
