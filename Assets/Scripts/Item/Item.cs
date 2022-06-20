
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
}
