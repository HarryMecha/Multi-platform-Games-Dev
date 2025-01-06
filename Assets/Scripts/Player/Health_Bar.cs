using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    #region Fields
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    #endregion

    //This sets up the healthbar Slider based on the player or enemies maxhealth
    public void SetMaxHealth(float health)
    {
        slider.maxValue=health;
        slider.value=health;

       fill.color= gradient.Evaluate(1f);
    }
    //This sets up the healthbar Slider to show the player or enemies current health
    public void SetHealth(float health)
    {
        slider.value= health;

        fill.color= gradient.Evaluate(slider.normalizedValue);
    }
}
