﻿using System;
using UnityEngine;

enum EnemyRangedState
{
    NotAlert,
    Recovering,
    Preparing
}
public class EnemyRanged : Enemy
{
    [SerializeField] private float prepareTime = 3f;
    [SerializeField] private float recoveryTime = 1f;
    [SerializeField] private GameObject projectile;
    
    private float _currentTime;

    private EnemyRangedState _state;
    
    new void Start()
    {
        base.Start();
        _currentTime = prepareTime;
        _state = EnemyRangedState.NotAlert;
        Animator.SetFloat(AnimIDSpeed, 0);
    }

    new void Update()
    {

        if (_state == EnemyRangedState.NotAlert)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, InitialOrientation, Time.deltaTime);
        }
        
        if (_state == EnemyRangedState.Preparing)
        {
            LookAtTarget();
            
            if ((_currentTime -= Time.deltaTime) <= 0)
            {
                Shoot();
            }    
            
            return;
        }
        
        var detectingPlayer = DetectTarget();

        if (detectingPlayer)
        {
            switch (_state)
            {
                case EnemyRangedState.NotAlert:
                    Prepare();
                    break;
                case EnemyRangedState.Recovering:
                {
                    if ((_currentTime -= Time.deltaTime) <= 0)
                    {
                        Prepare();
                    }

                    break;
                }
            }
        }
        else if (_state == EnemyRangedState.Recovering) // if the enemy is recovering after shooting, and player is out of distance, set to not alert
        {
            _state = EnemyRangedState.NotAlert;
        } // else, the enemy will keep preparing the shot, even if player goes out of the area
    }

    void Prepare()
    {
        Debug.Log("Preparing shot");
        _state = EnemyRangedState.Preparing;
        _currentTime = prepareTime;
    }

    void Shoot()
    {
        Debug.Log("Shot");
        _currentTime = recoveryTime;
        _state = EnemyRangedState.Recovering;

        var shotProjectile = Instantiate(projectile, transform.position + (Vector3.up * 1.5f) + transform.forward, Quaternion.identity);
        shotProjectile.GetComponent<Projectile>().ShootAt(GetTargetPos(), mindControlled);
    }

    private void OnDrawGizmos()
    {
        var iconPos = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        Gizmos.DrawIcon(iconPos, "Ranged.png", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}