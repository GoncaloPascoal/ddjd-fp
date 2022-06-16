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

    private Item _equip_item_sword;
    
    public Item EquipItemSword
    {
        get { return _equip_item_sword; }
        set { _equip_item_sword = value; }
    }
    
    private Item _equip_item_armour;
    
    public Item EquipItemArmour
    {
        get { return _equip_item_armour; }
        set { _equip_item_armour = value; }
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
        if (_is_on)
        {
            _inventory_manager.ShowItems();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown("OpenInventory"))
        {
            ToggleInventory();
        }

        if (_is_on)
        {
            if (Input.GetButtonDown("InvSortAll"))
            {
                _inventory_manager.ShowAllItems();
            }
            if (Input.GetButtonDown("InvSortConsumable"))
            {
                _inventory_manager.SetFilter("Sword");
                _inventory_manager.ShowItems();
            }
            if (Input.GetButtonDown("InvSortSword"))
            {
                _inventory_manager.SetFilter("Potion");
                _inventory_manager.ShowItems();
            }
            if (Input.GetButtonDown("InvSortArmour"))
            {
                _inventory_manager.SetFilter("Armour");
                _inventory_manager.ShowItems();
            }
            if (Input.GetButtonDown("SelectItem"))
            {
                Debug.Log("A");
                _inventory_manager.UseCurrentItem();
            }
            _inventory_manager.MoveCursor(Input.GetKeyDown(KeyCode.A), Input.GetKeyDown(KeyCode.D), Input.GetKeyDown(KeyCode.W), Input.GetKeyDown(KeyCode.S));
        }
    }

    void ToggleInventory()
    {
        _is_on = !_is_on;
        if (_is_on) InputManager.CurrentActionType = ActionType.Menu;
        else InputManager.CurrentActionType = ActionType.Game;
        _inventory_manager.gameObject.SetActive(_is_on);
        _inventory_manager.ShowAllItems();
    }

}
