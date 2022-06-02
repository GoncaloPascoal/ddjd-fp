using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<GameObject> _items;

    // Start is called before the first frame update
    void Start()
    {
        _items = new List<GameObject>();
    }

    public void AddItem(GameObject item)
    {
        _items.Add(item);
        item.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (_items.Count > 0)
        {
        }
        
    }
}
