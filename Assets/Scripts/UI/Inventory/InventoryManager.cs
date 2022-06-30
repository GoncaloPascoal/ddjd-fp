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
    private bool _equipped;

    private int _currentSlot;
    
    private List<Image> _slotIcons;
    private List<TMP_Text> _slotAmounts;
    private List<Item> _slotItems;

    private List<EquipmentDisplay> _equipmentDisplays;

    private Stats _playerStats;

    private MenuTabController _menuTabController;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        _playerStats = playerObj.GetComponent<Stats>();
        _inventory = playerObj.GetComponent<Inventory>();

        _equipped = false;
        _currentSlot = 0;

        _slotIcons = new List<Image>(inventorySlots);
        _slotAmounts = new List<TMP_Text>(inventorySlots);
        _slotItems = Enumerable.Repeat<Item>(null, inventorySlots).ToList();
        for (int i = 0; i < itemSlots.transform.childCount; ++i)
        {
            _slotIcons.Add(itemSlots.transform.GetChild(i).GetChild(0).GetComponent<Image>());
            _slotAmounts.Add(itemSlots.transform.GetChild(i).GetComponentInChildren<TMP_Text>());
        }

        _equipmentDisplays = new List<EquipmentDisplay>(equipmentSlots.transform.childCount);
        for (int i = 0; i < equipmentSlots.transform.childCount; ++i)
        {
            _equipmentDisplays.Add(equipmentSlots.transform.GetChild(i).GetComponent<EquipmentDisplay>());
        }

        _menuTabController = GetComponentInParent<MenuTabController>();
        UpdateInterface();
    }

    private void Update()
    {
        if (InputManager.Action("MenuBack").WasPressedThisFrame())
        {
            _menuTabController.Return();
            return;
        }

        if (InputManager.Action("InventoryToggleEquipped").WasPressedThisFrame())
        {
            ToggleEquipped();
        }

        if (InputManager.Action("MenuAction").WasPressedThisFrame())
        {
            ItemAction();
        }

        MoveCursor(
            InputManager.Action("MenuLeft").WasPressedThisFrame(),
            InputManager.Action("MenuRight").WasPressedThisFrame(),
            InputManager.Action("MenuUp").WasPressedThisFrame(),
            InputManager.Action("MenuDown").WasPressedThisFrame()
        );
    }

    private void OnEnable()
    {
        if (_inventory != null) UpdateInterface();
    }

    private void ToggleEquipped()
    {
        _equipped = !_equipped;
        _currentSlot = 0;
        UpdateCursorPosition();
        UpdateItemDisplay();
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

    public void UpdatePlayerStats()
    {
        for (int i = 0; i < playerStatDisplays.transform.childCount; ++i)
        {
            GameObject obj = playerStatDisplays.transform.GetChild(i).gameObject;
            StatDisplay display = obj.GetComponent<StatDisplay>();
            display.SetValue(_playerStats.GetStatValue(display.stat));
        }
    }

    private void UpdateItems()
    {
        int slot = 0;
        ISet<EquipmentSlot> slotsWithEquipment = new HashSet<EquipmentSlot>(); 

        foreach (List<Equipment> equipmentList in _inventory.Equipment.Values)
        {
            foreach (Equipment equipment in equipmentList)
            {
                if (!slotsWithEquipment.Contains(equipment.slot) && _inventory.Equipped[equipment.slot] == equipment)
                {
                    slotsWithEquipment.Add(equipment.slot);
                    continue;
                }

                if (slot == inventorySlots) return;
                _slotAmounts[slot].text = "";
                _slotIcons[slot].gameObject.SetActive(true);
                _slotIcons[slot].sprite = equipment.icon;
                _slotItems[slot++] = equipment;
            }
        }

        foreach (Consumable consumable in _inventory.Consumables.Keys)
        {
            if (slot == inventorySlots) return;
            uint quantity = _inventory.Consumables[consumable];
            _slotAmounts[slot].text = quantity > 1 ? quantity.ToString() : "";
            _slotIcons[slot].gameObject.SetActive(true);
            _slotIcons[slot].sprite = consumable.icon;
            _slotItems[slot++] = consumable;
        }

        while (slot != inventorySlots)
        {
            _slotAmounts[slot].text = "";
            _slotIcons[slot].gameObject.SetActive(false);
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
                    obj.SetActive(true);
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

        if (left || right || up || down)
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
