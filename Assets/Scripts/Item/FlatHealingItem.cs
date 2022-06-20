
using UnityEngine;

[CreateAssetMenu(fileName = "FlatHealingItem", menuName = "ScriptableObject/Item/Consumable/FlatHealingItem")]
public class FlatHealingItem : Consumable
{
    public int healing;

    public override void Use()
    {
        GameObject.FindWithTag("Player").GetComponent<Damageable>().ChangeHealth(healing);
    }
}