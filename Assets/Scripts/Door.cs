using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using StarterAssets;
using UnityEngine;

public class Door : MonoBehaviour, Activatable
{

    private Animator _animator;

    private DoorSounds _ds;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _ds = GetComponent<DoorSounds>();
        Debug.Log(_ds);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void activate()
    {
        _animator.SetTrigger("Open");
        _ds.OpenDoorSound();
    }

    public void deactivate()
    {
        
    }
}
