using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private float updateSpeed = 1.0f;

    private Slider _slider;
    private float _targetValue, _updateRate;
    private const float Epsilon = 0.01f;
    
    protected virtual void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _targetValue = _slider.value;
        CalculateUpdateRate();
    }

    private void Update()
    {
        if (Math.Abs(_slider.value - _targetValue) > Epsilon)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _targetValue, _updateRate * Time.deltaTime);
        }
    }

    public virtual void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = Math.Min(_slider.value, maxValue);
        CalculateUpdateRate();
    }

    public void SetValue(float value)
    {
        _targetValue = value;
    }

    public void SetValueInstantly(float value)
    {
        _slider.value = value;
        _targetValue = value;
    }

    private void CalculateUpdateRate()
    {
        _updateRate = _slider.maxValue * updateSpeed;
    }
}
