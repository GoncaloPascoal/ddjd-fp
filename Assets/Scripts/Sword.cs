using UnityEngine;

public class Sword : Item
{
    [SerializeField] private int damage;
    
    public virtual void Equip()
    {
        Debug.Log("Damage increase: " + damage);
        return;
    }
    
    public virtual void Unequip()
    {
        Debug.Log("Damage decrease: " + damage);
        return;
    }
}