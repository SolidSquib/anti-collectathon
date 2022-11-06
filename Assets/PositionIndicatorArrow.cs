using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionIndicatorArrow : MonoBehaviour
{
    public Transform m_target;
    public Player m_player;

    public Image m_image;
    private Canvas m_canvas;

    private void Start()
    {
        m_canvas = FindObjectOfType<Canvas>();

        RectTransform rectTransform = transform as RectTransform;
        if (rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
        }
    }

    private void Update()
    {
        if (m_target != null && m_player != null)
        {
            Vector2 deltaVector = m_target.position - m_player.transform.position;
            Vector2 directionToTarget = deltaVector.normalized;

            if (!m_target.GetComponent<Renderer>().isVisible)
            {
                m_image.enabled = true;

                float angle = Mathf.DeltaAngle(Quaternion.Euler(Vector2.up).eulerAngles.z, Quaternion.LookRotation(Vector3.forward, directionToTarget).eulerAngles.z);
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);                
            }
            else
            {
                m_image.enabled = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }

        float bufferZone = (transform as RectTransform).rect.width + 5.0f;
        Vector2 targetScreenLocation = RectTransformUtility.WorldToScreenPoint(Camera.main, m_target.transform.position);
                
        transform.position = new Vector3(Mathf.Clamp(targetScreenLocation.x, bufferZone, Screen.width - bufferZone), Mathf.Clamp(targetScreenLocation.y, bufferZone, Screen.height - bufferZone), 0.0f);
    }
}
