using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderInArea : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> objects;
    
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
        foreach (var objectRender in objects)
        {
            objectRender.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var objectRender in objects)
        {
            objectRender.enabled = false;
        }
    }
}
