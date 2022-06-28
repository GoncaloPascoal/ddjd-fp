using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject activatable;

    [SerializeField] private bool willActivate;
    
    private Animator _animator;

    private GameSaveManager _saveManager;
    // Start is called before the first frame update
    void Start()
    {
        _saveManager = GetComponent<GameSaveManager>();
        _animator = GetComponent<Animator>();

        if (GameData.PressurePlatesActivated.Contains(GameData.GameObjectToHash(gameObject)))
        {
            activatable.GetComponent<Activatable>().Activate();
        }
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
                GameData.AddActivatedPressurePlate(this);
                activatable.GetComponent<Activatable>().Activate();
            }
            else
            {
                activatable.GetComponent<Activatable>().Deactivate();
            }
        }
    }

    
}
