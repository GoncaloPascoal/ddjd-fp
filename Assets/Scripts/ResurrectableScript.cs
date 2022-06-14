using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectableScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _souls;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void FinishedResurrection()
    {
        _souls.GetComponent<FloatingSoulScript>().EndResurrection();
    }
}
