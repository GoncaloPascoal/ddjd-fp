using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ProjectileSpell : MonoBehaviour
{
    private Vector3 _position;

    private float _timeBeforeShoot;

    [SerializeField] private float timeBeforePosition;
    [SerializeField] private float intoPositionSpeed = 10f;
    [SerializeField] private float shootSpeed = 20f;
    [SerializeField] private float damage = 10f;
    private BossSounds _bs;

    private GameObject _target;
    private Vector3 _shootDirection;

    private float _timePassed = 0f;
    private bool _shot = false;

    private void Start()
    {
        _bs = GetComponent<BossSounds>();
    }

    public void InitializeWithPositionTarget(Vector3 position, GameObject target, float timeBeforeShoot)
    {
        _position = position;
        _target = target;
        _timeBeforeShoot = timeBeforeShoot + timeBeforePosition;
    }

    private void Update()
    {
        if (_shot)
        {
            transform.position += _shootDirection * shootSpeed * Time.deltaTime;
                // Vector3.Lerp(transform.position, _targetCurrentPos + Vector3.up, Time.deltaTime * shootSpeed);
        }
        else if (_timePassed >= timeBeforePosition && _timePassed < _timeBeforeShoot)
        {
            transform.position = Vector3.Lerp(transform.position, _position, Time.deltaTime * intoPositionSpeed);
        } 
        else if (_timePassed >= _timeBeforeShoot)
        {
            Shoot();
        }

        _timePassed += Time.deltaTime;
    }

    public void Shoot()
    {
        _shot = true;
        _shootDirection = (_target.transform.position + Vector3.up) - transform.position;
        _bs.ProjectileSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Hittable>().Hit(damage);
            Destroy(gameObject);
        }
    }
}
