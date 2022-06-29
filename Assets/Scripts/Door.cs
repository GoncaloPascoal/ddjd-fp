using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using StarterAssets;
using UnityEngine;

public class Door : MonoBehaviour, Activatable
{
    private Animator _animator;

    private DoorSounds _ds;
    private bool _setToActivate = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _ds = GetComponent<DoorSounds>();
        if (_setToActivate)
            Activate();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Activate()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Open");
            _ds.OpenDoorSound();
        }
        else
            _setToActivate = true;
    }

    public void Deactivate()
    {
        
    }
}
