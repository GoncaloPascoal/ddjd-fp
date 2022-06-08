using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Inventory _inventory;

    [SerializeField]
    private GameObject _item_slots_obj;

    private List<Image> _slots = new List<Image>();

    [SerializeField]
    private Sprite _default_icon;


    private List<Item> _currently_shown_items = new List<Item>();

    private string _current_filter = "";

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _item_slots_obj.transform.childCount; i++)
        {
            _slots.Add(_item_slots_obj.transform.GetChild(i).GetComponent<Image>());
        }
        ShowItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("tmp1"))
        {
            FilterItems("Sword");
            ShowItems();
        }
        if (Input.GetButtonDown("tmp2"))
        {
            FilterItems("Potion");
            ShowItems();
        }
    }

    public void FilterItems(string item_type)
    {
        if (item_type == "")
        {
            FilterItems();
            return;
        }
        _current_filter = item_type;
        InvItem.ItemType _enum_item_type = (InvItem.ItemType) System.Enum.Parse(typeof(InvItem.ItemType), item_type);

        _currently_shown_items.Clear();
        foreach(Item _item in _inventory.Items)
        {
            if(_item.InvItem.itemType == _enum_item_type)
            {
                _currently_shown_items.Add(_item);
            }
        }

    }

    public void FilterItems()
    {
        _current_filter = "";
        _currently_shown_items.Clear();
        foreach (Item item in _inventory.Items)
        {
            _currently_shown_items.Add(item);
        }
    }

    public void FilterAndShowItems(string itemType)
    {
        Debug.Log(itemType);
        FilterItems(itemType);
        ShowItems();
    }

    public void HelloWorld()
    {
        Debug.Log(("Hello, World!"));
    }
    
    public void ShowAllItems()
    {
        FilterItems();
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_currently_shown_items.Count > i)
            {
                _slots[i].sprite = _currently_shown_items[i].InvItem.icon;
            }
            else
            {
                _slots[i].sprite = _default_icon;
            }
        }
    }

    public void ShowItems()
    {
        FilterItems(_current_filter);
        for (int i = 0; i < _slots.Count; i++)
        {
            if(_currently_shown_items.Count > i)
            {
                _slots[i].sprite = _currently_shown_items[i].InvItem.icon;
            }
            else
            {
                _slots[i].sprite = _default_icon;
            }
        }
    }
}
