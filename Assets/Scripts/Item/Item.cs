
using System;
using UnityEngine;
using Object = System.Object;

[Serializable]
public abstract class Item : ScriptableObject
{
    public string itemName, itemNamePlural;
    [TextArea] public string description;
    public Sprite icon;

    public override bool Equals(Object obj)
    {
        return itemName == (obj as Item).itemName;
    }
}
