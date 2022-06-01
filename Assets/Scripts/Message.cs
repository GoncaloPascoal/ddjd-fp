using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private string _labelText;

    private bool _colliding;
    // Start is called before the first frame update
    void Start()
    {
        _colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnGUI()
    {
        if (_colliding)
        {
            GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),(_labelText));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _colliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _colliding = false;
        }
    }
}
