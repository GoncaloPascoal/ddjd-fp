using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemPickupDictionary : SerializableDictionary<Item, uint> { }

public class Pickup : MonoBehaviour
{
    private string _labelText;

    [SerializeField] private ItemPickupDictionary items;
    private Inventory _inventory;

    private bool _active;

    private void Start()
    {
        _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        _labelText = "Hit E to pick up";
        _active = false;
    }

    private void Update()
    {
        if (!_active) return;;

        if (Input.GetButtonDown("Interact"))
        {
            foreach (Item item in items.Keys)
            {
                _inventory.AddItem(item, items[item]);
            }
            items.Clear();
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        if (_active)
        {
            GUI.Box(new Rect(140, Screen.height - 50, Screen.width - 300, 120), _labelText);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _active = false;
        }
    }
}
