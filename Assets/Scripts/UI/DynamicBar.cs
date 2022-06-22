
using System;
using UnityEngine;

public class DynamicBar : Bar
{
    [SerializeField] private float widthPerUnit = 3.0f;

    private RectTransform _rectTransform;

    protected override void Awake()
    {
        base.Awake();
        _rectTransform = GetComponent<RectTransform>();
    }

    public override void SetMaxValue(float maxValue)
    {
        base.SetMaxValue(maxValue);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxValue * widthPerUnit);
    }
}