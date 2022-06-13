using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Selectable
{
    [SerializeField]
    private Inventory _inventory;

    [SerializeField]
    private GameObject _item_slots_parent;

    [SerializeField] private GameObject _item_slot_prefab;
    
    private List<Image> _slots = new List<Image>();

    [SerializeField]
    private Sprite _default_icon;

    private bool isFiltering = false;
    private InvItem.ItemType _current_filter = 0;

    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _item_slots_parent.transform.childCount; i++)
        {
            _slots.Add(_item_slots_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>());
        }
        //ShowItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInventory(Inventory inv)
    {
        inventory = inv;
    }

    public void SetFilter(string itemType)
    {
        if (itemType == "")
        {
            isFiltering = false;
            _current_filter = 0;
            return;
        }
        SetFilter((InvItem.ItemType) System.Enum.Parse(typeof(InvItem.ItemType), itemType));
    }

    public void SetFilter(InvItem.ItemType filter)
    {
        _current_filter = filter; 
        isFiltering = true;
    }

    void ShowNothing()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void ShowItems()
    {

        if (!isFiltering)
        {
            ShowAllItems();
        }

        if (!inventory.Items.ContainsKey(_current_filter))
        {
            ShowNothing();
            return;
        }
        
        List<Item> items = inventory.Items[_current_filter];

        if (items == null)
            return;
        
        SetSlots(items.Count);
        
        for (int i = 0; i < _slots.Count; i++)
        {
            if (items.Count > i)
            {
                _slots[i].transform.parent.gameObject.SetActive(true);
                _slots[i].sprite = items[i].InvItem.icon;
            }
            else
            {
                _slots[i].transform.parent.gameObject.SetActive(false);
                //_slots[i].sprite = _default_icon;
            }
        }
        
    }

    public void ShowAllItems()
    {
        isFiltering = false;
        int i = 0;
        Debug.Log("Num buttons: " + _slots.Count);
        foreach (var invItem in inventory.Items)
        {
            for (int j = 0; j < invItem.Value.Count; j++)
            {
                if (_slots.Count > i)
                {
                    _slots[i].transform.parent.gameObject.SetActive(true);
                    _slots[i].sprite = invItem.Value[j].InvItem.icon;
                }
                else
                {
                    AddSlot();
                    _slots[i].transform.parent.gameObject.SetActive(true);
                    _slots[i].sprite = invItem.Value[j].InvItem.icon;
                }
                i++; 
            }
            
        }

        for (int j = i; j < _slots.Count; j++)
        {
            _slots[j].transform.parent.gameObject.SetActive(false);
        }
        
        if(_slots.Count > 0){
            if (_slots[0].transform.parent.gameObject.activeSelf)
            {
                _slots[0].transform.parent.gameObject.GetComponent<Button>().Select();
            }
        }
    }

    public void AddSlot()
    {
        var newItemSlot = Instantiate(_item_slot_prefab, _item_slots_parent.transform);
        _slots.Add(newItemSlot.transform.GetChild(0).GetComponent<Image>());
    }
    
    public void SetSlots(int numSlots)
    {
        if (numSlots == _slots.Count)
            return;
        while (numSlots > _slots.Count)
        {
            AddSlot();
        }
    }
    
}
