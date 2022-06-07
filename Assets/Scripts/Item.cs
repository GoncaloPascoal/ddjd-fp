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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

