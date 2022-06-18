using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject activatable;

    [SerializeField] private bool willActivate;
    
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    { 
        _animator = GetComponent<Animator>();
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
                _animator.SetTrigger("Press");
                activatable.GetComponent<Activatable>().activate();
            }
            else
            {
                activatable.GetComponent<Activatable>().deactivate();
            }
        }
    }

    
}
