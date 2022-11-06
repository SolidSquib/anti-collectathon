using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthDisplay : MonoBehaviour
{
    public Image backgroundImage;
    public Text healthText;

    public void NewHealthValue(int count)
    {
        healthText.text = count.ToString();
    }
}
