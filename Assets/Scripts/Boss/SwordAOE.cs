using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SwordAOE : MonoBehaviour
{
    [SerializeField] private float timeInterval = 1f;
    [SerializeField] private float distanceBetween = 2f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageRate = 1f;
    
    private int _numberOfBatches = 0;
    private Vector3 _direction;
    private float _interval = 0f;
    private bool _expanded = false;

    private float damageTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitializeWithDirection(Vector3 direction, int numberBatches)
    {
        _numberOfBatches = numberBatches - 1;
        _direction = direction;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (!_expanded && _numberOfBatches > 0 && _interval >= timeInterval)
        {
            GameObject newAoeObject = Instantiate(gameObject, transform.position + _direction * distanceBetween, Quaternion.LookRotation(_direction));
            newAoeObject.GetComponent<SwordAOE>().InitializeWithDirection(_direction, _numberOfBatches);
            _expanded = true;
        }

        if (_interval >= duration)
        {
            Destroy(gameObject);
        }

        _interval += Time.deltaTime;
        damageTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Hittable>().Hit(damage);
            damageTime = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && damageTime >= damageRate)
        {
            Debug.Log("trigger stay");
            other.GetComponent<Hittable>().Hit(damage);
            damageTime = 0f;
        }
    }
}
