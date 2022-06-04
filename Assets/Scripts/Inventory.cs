using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    private List<Item> _items;

    // Start is called before the first frame update
    void Start()
    {   
        _items = new List<Item>();
    }

    public void AddItem(GameObject item)
    {
        _items.Add(item.GetComponent<Item>());
        //Make it invisible after picking it up
        item.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
