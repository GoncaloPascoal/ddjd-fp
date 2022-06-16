using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private InvItem _inv_item;

    public InvItem InvItem
    {
        get { return _inv_item; }
    }

    void Start()
    {
    }

    public void Print()
    {
        //Debug.Log(name);
    }

    public virtual bool Use()
    {
        Destroy(gameObject);
        return true;
    }
    
    public virtual void Equip()
    {
        Debug.Log("bruh 1");
        return;
    }
    
    public virtual void Unequip()
    {
        Debug.Log("bruh");
        return;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

