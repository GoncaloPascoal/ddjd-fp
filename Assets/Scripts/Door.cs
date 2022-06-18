using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using StarterAssets;
using UnityEngine;

public class Door : MonoBehaviour, Activatable
{

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

    public void activate()
    {
        _animator.SetTrigger("Open");
    }

    public void deactivate()
    {
        
    }
}
