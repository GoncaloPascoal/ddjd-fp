
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName, itemNamePlural;
    [TextArea] public string description;
    public Sprite icon;
}
