using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image health;
    public TextMeshProUGUI valueText;
    public void UpdateHealth(float currenthealth, float maxHealth)
    {
        health.fillAmount = currenthealth / maxHealth;
        valueText.text = currenthealth.ToString() +" / "+ maxHealth.ToString();
    }
}
