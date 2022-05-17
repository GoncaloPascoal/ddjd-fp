using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Backstab : MonoBehaviour
{
    private ThirdPersonController _player;
    private BoxCollider _backStabArea;

    [SerializeField] private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        _backStabArea = GetComponent<BoxCollider>();
        _player = GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.SetBackstabTarget(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.RemoveBackstabTarget(enemy);
        }
    }
}
