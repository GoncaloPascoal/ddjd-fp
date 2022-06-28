using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using StarterAssets;
using UnityEngine;

public class Door : MonoBehaviour, Activatable
{
    private Animator _animator;

    private bool _setToActivate = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
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
            _animator.SetTrigger("Open");
        else
            _setToActivate = true;
    }

    public void Deactivate()
    {
        
    }
}
