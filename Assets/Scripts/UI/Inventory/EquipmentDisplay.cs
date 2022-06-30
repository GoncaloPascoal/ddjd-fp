using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDisplay : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] public EquipmentSlot slot;

    public void UpdateDisplay(Inventory inventory)
    {
        Equipment equipped = inventory.Equipped[slot];
        icon.gameObject.SetActive(equipped != null);
        if (equipped != null) icon.sprite = equipped.icon;
    }
}
