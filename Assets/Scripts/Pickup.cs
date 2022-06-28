using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemPickupDictionary : SerializableDictionary<Item, uint> { }

public class Pickup : MonoBehaviour
{
    private static readonly string[] PromptButtons = { "E", "(A)" };
    private const string PromptAction = "Pick Up Item";

    [SerializeField] private ItemPickupDictionary items;
    private Inventory _inventory;
    private GameSaveManager _saveManager;

    private bool _active;

    private void Start()
    {
        _saveManager = GetComponent<GameSaveManager>();
        _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        _active = false;
    }

    private void Update()
    {
        if (!_active) return;

        if (Input.GetButtonDown("Interact"))
        {
            HUD.Instance.HideButtonPrompt();
            HUD.Instance.ShowItemPickup(items);
            foreach (Item item in items.Keys)
            {
                _inventory.AddItem(item, items[item]);
                GameData.AddPickedUpItem(item);
            }
            items.Clear();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _active = true;
            HUD.Instance.ShowButtonPrompt(PromptButtons, PromptAction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _active = false;
            HUD.Instance.HideButtonPrompt();
        }
    }
}
