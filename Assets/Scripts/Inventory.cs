using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    private Dictionary<InvItem.ItemType, List<Item>> _items = new Dictionary<InvItem.ItemType, List<Item>>();

    public Dictionary<InvItem.ItemType, List<Item>> Items
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
        _inventory_manager.SetInventory(this);
    }

    public void AddItem(GameObject item)
    {
        Item itemScript = item.GetComponent<Item>();
        InvItem.ItemType itemType = itemScript.InvItem.itemType;
        if (!_items.ContainsKey(itemType))
        {
            List<Item> catItemList = new List<Item>();
            catItemList.Add(itemScript);
            _items[itemType] = catItemList;
        }
        else
        {
            _items[itemType].Add(itemScript);
        }
        //Make it invisible after picking it up
        item.SetActive(false);
        _inventory_manager.ShowItems();
    }
    // Update is called once per frame
    void Update()
    {
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
