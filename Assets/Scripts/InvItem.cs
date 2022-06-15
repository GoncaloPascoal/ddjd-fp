using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Custom/Item/Create New Item")]
public class InvItem : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;

    public enum ItemType
    {
        Potion,
        Sword,
        Other
    }
}
