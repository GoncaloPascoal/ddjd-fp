using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private Slider _slider;
    
    private void Start()
    {
        _slider = GetComponentInChildren<Slider>();
    }

    public void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }
}
