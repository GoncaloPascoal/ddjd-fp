using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Inventory _inventory;

    [SerializeField]
    private GameObject _item_slots_parent;

    [SerializeField] private GameObject _item_slot_prefab;
    
    
    private List<Item> _slot_items = new List<Item>();
    private List<Image> _slots = new List<Image>();

    private int _slot = 0;

    [SerializeField] private int _slots_per_row = 6;
    
    [SerializeField] private int _slots_in_inventory = 36;

    [SerializeField]
    private Sprite _default_icon;

    private bool isFiltering = false;
    private InvItem.ItemType _current_filter = 0;

    private Inventory inventory;

    [Header("Description parts")] [SerializeField]
    private Image descImage;
    [SerializeField]
    private Text descTitle;
    [SerializeField]
    private Text descText;
    

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _item_slots_parent.transform.childCount; i++)
        {
            _slots.Add(_item_slots_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>());
        }
        ShowAllItems();
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
            _slots[i].gameObject.SetActive(false);
            _slot_items.Clear();
        }
        
        ShowDescription();
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
        _slot_items.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            _slot_items.Add(items[i]);
        }

        if (items == null)
            return;
        
        
        for (int i = 0; i < _slots.Count; i++)
        {
            if (items.Count > i)
            {
                _slots[i].gameObject.SetActive(true);
                _slots[i].sprite = items[i].InvItem.icon;
            }
            else
            {
                Debug.Log("A");
                _slots[i].gameObject.SetActive(false);
            }
        }
        
        ShowDescription();
    }

    public void ShowAllItems()
    {
        isFiltering = false;
        int i = 0;
        
        _slot_items.Clear();
        foreach (var invItem in inventory.Items)
        {
            for (int j = 0; j < invItem.Value.Count; j++)
            {
                if (_slots.Count > i)
                {
                    _slots[i].gameObject.SetActive(true);
                    _slots[i].sprite = invItem.Value[j].InvItem.icon;
                    _slot_items.Add(invItem.Value[j]);
                }
                i++; 
            }
            
        }

        for (int j = i; j < _slots.Count; j++)
        {
            _slots[j].gameObject.SetActive(false);
        }

        ShowDescription();
    }

    private void ShowDescription()
    {
        if (_slot_items.Count > _slot)
        {
            descImage.gameObject.SetActive(true);
            descImage.sprite = _slots[_slot].sprite;
            descTitle.text = _slot_items[_slot].InvItem.itemName;
            descText.text = _slot_items[_slot].InvItem.description;
        }
        else
        {
            descImage.gameObject.SetActive(false);
            descTitle.text = "";
            descText.text = "";
        }
    }
    
    public void MoveCursor(bool left, bool right, bool up, bool down){
        if (left)
        {
            _slot -= 1;
            if (_slot < 0)
            {
                _slot += _slots_per_row;
            }
            else if (_slot % _slots_per_row == 5)
            {
                _slot += _slots_per_row;
            }
        }
        if (right)
        {
            _slot += 1;
            if (_slot % _slots_per_row == 0)
            {
                _slot -= 6;
            }
        }  
        if (up)
        {
            _slot -= _slots_per_row;
            if (_slot < 0)
            {
                _slot += _slots_in_inventory;
            }
        }
        if (down)
        {
            _slot += _slots_per_row;
            _slot = _slot % _slots_in_inventory;
        }  
        Debug.Log(_slot);
        if(up || down || left || right)
            ShowDescription();
    }
    
}
