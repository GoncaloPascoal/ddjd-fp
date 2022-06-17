using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemSlots;

    [SerializeField] private int slotsPerRow = 5;
    [SerializeField] private int inventorySlots = 30;
    [SerializeField] private Image cursor;
    [SerializeField] private Sprite defaultIcon;

    [Header("Equipment Panel")]
    [SerializeField] private int equipmentSlotsPerRow = 2;
    [SerializeField] private GameObject equipmentSlots;
    [SerializeField] private GameObject playerStatDisplays;

    [Header("Description Panel")]
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private GameObject statsPanel, statDisplays;

    private Inventory _inventory;
    private bool _visible, _equipped, _filtering;
    private int _currentSlot;
    private List<Image> _slotIcons;
    private List<Item> _slotItems;

    private List<EquipmentDisplay> _equipmentDisplays;

    private ThirdPersonController _player;
    
    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        _player = playerObj.GetComponent<ThirdPersonController>();
        _inventory = playerObj.GetComponent<Inventory>();

        _visible = false;
        _equipped = false;
        _filtering = false;
        _currentSlot = 0;

        _slotIcons = new List<Image>(inventorySlots);
        _slotItems = Enumerable.Repeat<Item>(null, inventorySlots).ToList();
        for (int i = 0; i < itemSlots.transform.childCount; ++i)
        {
            _slotIcons.Add(itemSlots.transform.GetChild(i).GetChild(0).GetComponent<Image>());
        }

        _equipmentDisplays = new List<EquipmentDisplay>(equipmentSlots.transform.childCount);
        for (int i = 0; i < equipmentSlots.transform.childCount; ++i)
        {
            _equipmentDisplays.Add(equipmentSlots.transform.GetChild(i).GetComponent<EquipmentDisplay>());
        }

        UpdateInterface();
    }

    private void Update()
    {
        if (InputManager.GetButtonDown("ToggleInventory"))
        {
            ToggleInventory();
        }

        // if (Input.GetButtonDown("InvSortAll"))
        // {
        //     ShowAllItems();
        // }
        // if (Input.GetButtonDown("InvSortConsumable"))
        // {
        //     SetFilter("Sword");
        //     ShowItems();
        // }
        // if (Input.GetButtonDown("InvSortSword"))
        // {
        //     SetFilter("Potion");
        //     ShowItems();
        // }
        // if (Input.GetButtonDown("InvSortArmour"))
        // {
        //     SetFilter("Armour");
        //     ShowItems();
        // }

        if (InputManager.GetButtonDown("InventoryToggleEquipped"))
        {
            ToggleEquipped();
        }

        if (InputManager.GetButtonDown("InventoryItemAction"))
        {
            ItemAction();
        }

        MoveCursor(InputManager.GetButtonDown("MenuLeft"), InputManager.GetButtonDown("MenuRight"),
            InputManager.GetButtonDown("MenuUp"), InputManager.GetButtonDown("MenuDown"));
    }

    private void ToggleInventory()
    {
        _visible = !_visible;
        InputManager.CurrentActionType = _visible ? ActionType.Menu : ActionType.Game;
        transform.GetChild(0).gameObject.SetActive(_visible);
        UpdateInterface();
    }

    private void ToggleEquipped()
    {
        _equipped = !_equipped;
        _currentSlot = 0;
        UpdateCursorPosition();
    }

    private void UpdateInterface()
    {
        UpdateItems();
        UpdateEquipmentDisplay();
        UpdateItemDisplay();
        UpdatePlayerStats();
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        cursor.transform.position = _equipped ?
            _equipmentDisplays[_currentSlot].transform.position :
            _slotIcons[_currentSlot].transform.position;
    }

    private void UpdateEquipmentDisplay()
    {
        foreach (EquipmentDisplay display in _equipmentDisplays)
        {
            display.UpdateDisplay(_inventory);
        }
    }

    private void UpdatePlayerStats()
    {
        for (int i = 0; i < playerStatDisplays.transform.childCount; ++i)
        {
            GameObject obj = playerStatDisplays.transform.GetChild(i).gameObject;
            StatDisplay display = obj.GetComponent<StatDisplay>();
            display.SetValue(_player.GetStatValue(display.stat));
        }
    }

    // public void SetFilter(string itemType)
    // {
    //     if (itemType == "")
    //     {
    //         _filtering = false;
    //         _current_filter = 0;
    //         return;
    //     }
    //     SetFilter((Item.ItemType) System.Enum.Parse(typeof(Item.ItemType), itemType));
    // }
    //
    // public void SetFilter(Item.ItemType filter)
    // {
    //     _current_filter = filter; 
    //     _filtering = true;
    // }

    private void UpdateItems()
    {
        int slot = 0;

        foreach (List<Equipment> equipmentList in _inventory.Equipment.Values)
        {
            foreach (Equipment equipment in equipmentList)
            {
                if (_inventory.Equipped[equipment.slot] == equipment) continue;

                if (slot == inventorySlots) return;
                _slotIcons[slot].sprite = equipment.icon;
                _slotItems[slot++] = equipment;
            }
        }

        foreach (Consumable consumable in _inventory.Consumables.Keys)
        {
            if (slot == inventorySlots) return;
            _slotIcons[slot].sprite = consumable.icon;
            _slotItems[slot++] = consumable;
        }

        while (slot != inventorySlots)
        {
            _slotIcons[slot].sprite = defaultIcon;
            _slotItems[slot++] = null;
        }
    }

    private void UpdateItemDisplay()
    {
        DisplayItem(_equipped ? _inventory.Equipped[_equipmentDisplays[_currentSlot].slot] : _slotItems[_currentSlot]);
    }

    private void DisplayItem(Item item)
    {
        if (item == null)
        {
            HideItemDisplay();
            return;
        }

        itemContainer.SetActive(true);
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        itemIcon.sprite = item.icon;

        if (item is Equipment equipment)
        {
            statsPanel.SetActive(true);

            for (int i = 0; i < statDisplays.transform.childCount; ++i)
            {
                GameObject obj = statDisplays.transform.GetChild(i).gameObject;
                StatDisplay display = obj.GetComponent<StatDisplay>();
                if (equipment.GetStatValue(display.stat) != 0)
                {
                    display.SetValue(equipment.GetStatValue(display.stat));
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
        else
        {
            statsPanel.SetActive(false);
        }
    }

    private void HideItemDisplay()
    {
        itemContainer.SetActive(false);
    }

    private void MoveCursor(bool left, bool right, bool up, bool down)
    {
        int row = _equipped ? equipmentSlotsPerRow : slotsPerRow;
        int total = _equipped ? _equipmentDisplays.Count : inventorySlots;

        if (left)
        {
            _currentSlot -= 1;
            if (_currentSlot < 0 || _currentSlot % row == row - 1) _currentSlot += row;
        }

        if (right)
        {
            _currentSlot += 1;
            if (_currentSlot % row == 0) _currentSlot -= row;
        }

        if (up)
        {
            _currentSlot -= row;
            if (_currentSlot < 0) _currentSlot += total;
        }

        if (down)
        {
            _currentSlot = (_currentSlot + row) % total;
        }

        if (up || down || left || right)
        {
            UpdateCursorPosition();
            UpdateItemDisplay();
        }
    }

    private void ItemAction()
    {
        Item currentItem = _equipped ? _inventory.Equipped[_equipmentDisplays[_currentSlot].slot] : _slotItems[_currentSlot];
        if (currentItem == null) return;

        switch (currentItem)
        {
            case Consumable consumable:
                _inventory.Use(consumable);
                break;
            case Equipment equipment:
                if (_equipped)
                    _inventory.Unequip(equipment.slot);
                else
                    _inventory.Equip(equipment);
                break;
        }

        UpdateInterface();
    }
}
