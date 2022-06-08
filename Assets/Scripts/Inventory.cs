using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    private List<Item> _items = new List<Item>();

    public List<Item> Items
    {
        get { return _items; }
    }

    private bool _is_on = false;

    [SerializeField]
    private InventoryManager _inventory_manager;

    // Start is called before the first frame update
    void Start()
    {   
        _inventory_manager.gameObject.SetActive(false);
    }

    public void AddItem(GameObject item)
    {
        _items.Add(item.GetComponent<Item>());
        //Make it invisible after picking it up
        item.SetActive(false);
        _inventory_manager.ShowItems();
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Item item in _items)
        {
            item.Print();
        }

        if (Input.GetButtonDown("OpenInventory"))
        {
            ToggleInventory();
        }

    }

    void ToggleInventory()
    {
        _is_on = !_is_on;
        _inventory_manager.gameObject.SetActive(_is_on);
        _inventory_manager.ShowAllItems();
    }

}
