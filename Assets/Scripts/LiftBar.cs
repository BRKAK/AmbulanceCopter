using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiftBar : MonoBehaviour
{
    public Slider slider;
    public void SetValue(float x)
    {
        slider.value = x;
    }
    public void setMaxValue(float x)
    {
        slider.maxValue = x;
    }
    public void setMinValue(float x)
    {
        slider.minValue = x;
    }
}
