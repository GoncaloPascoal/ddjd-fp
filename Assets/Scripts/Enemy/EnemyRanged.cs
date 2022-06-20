using System;
using System.Collections.Generic;
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
    
    private new void Start()
    {
        base.Start();
        _currentTime = prepareTime;
        _state = EnemyRangedState.NotAlert;
    }

    private new void Update()
    {
        if (backstabbed || gameObject.CompareTag("Dead")) return;

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

        bool detectingPlayer = DetectTarget();

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
    
    protected override void ChangeTargetsMindControl(List<string> newTargets)
    {
        // TODO: enemy ranged should have list with target tags that it deals damage to
        
        throw new NotImplementedException(); 
    }

    void Prepare()
    {
        Debug.Log("Preparing shot");
        _animator.SetTrigger("Aim");
        _state = EnemyRangedState.Preparing;
        _currentTime = prepareTime;
    }

    private void Shoot()
    {
        _currentTime = recoveryTime;
        _state = EnemyRangedState.Recovering;

        _animator.SetTrigger("Shoot");

        GameObject shotProjectile = Instantiate(projectile, transform.position + (Vector3.up * 1.5f) + transform.forward, Quaternion.identity);
        shotProjectile.GetComponent<Projectile>().ShootAt(GetTargetPos(), (int) Stats.GetStatValue(StatName.Damage), mindControlled);
    }

    private void OnDrawGizmos()
    {
        Vector3 iconPos = transform.position + Vector3.up * 2.5f;
        Gizmos.DrawIcon(iconPos, "Ranged.png", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}