using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AmmoDisplay : MonoBehaviour
{
    public Image m_NextAmmoTypeIcon;
    public Text m_AmmoText;

    public void NextAmmoType (Color newColour)
    {
        m_NextAmmoTypeIcon.color = newColour;
    }

    public void NewAmmoCount(int count)
    {
        m_AmmoText.text = count.ToString();
    }
}
