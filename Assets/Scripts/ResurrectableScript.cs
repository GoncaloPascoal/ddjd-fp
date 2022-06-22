using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectableScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _souls;

    public void FinishedResurrection()
    {
        _souls.GetComponent<FloatingSoul>().EndResurrection();
    }
}
