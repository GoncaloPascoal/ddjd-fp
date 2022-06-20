using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] public Bar healthBar, staminaBar;
    public static HUD Instance;

    private void Awake()
    {
        Instance = this;
    }
}
