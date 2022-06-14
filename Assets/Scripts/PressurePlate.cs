using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject activatable;

    [SerializeField] private bool willActivate;
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (willActivate)
            {
                activatable.GetComponent<Activatable>().activate();
            }
            else
            {
                activatable.GetComponent<Activatable>().deactivate();
            }
        }
    }

    
}
