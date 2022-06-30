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
    private EnemySounds _sounds;

    private new void Start()
    {
        base.Start();
        _state = EnemyRangedState.NotAlert;
        _sounds = GetComponent<EnemySounds>();
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
            return;
        }

        bool detectedTarget = DetectTarget();

        if (detectedTarget)
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

    public void Stagger()
    {
        // Got staggered in the middle of a shot, reset state
        _state = EnemyRangedState.NotAlert;
    }

    protected override void ChangeTargetsMindControl(List<string> newTargets)
    {
        // Don't need to do any changes
    }

    private void Prepare()
    {
        _animator.SetTrigger("Aim");
        _state = EnemyRangedState.Preparing;
        _currentTime = prepareTime;
    }

    private void Shoot()
    {
        _currentTime = recoveryTime;
        _state = EnemyRangedState.Recovering;

        _animator.SetTrigger("Shoot");
        _sounds.ShootBow();

        Vector3? targetPos = GetTargetPos();
        if (targetPos != null)
        {
            GameObject projectileInstance = Instantiate(projectile, transform.position + (Vector3.up * 1.5f) + transform.forward, Quaternion.identity);
            projectileInstance.GetComponent<Projectile>().ShootAt((Vector3) targetPos, Stats.GetStatValue(StatName.Damage), mindControlled);
        }
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

    public override void StopMovement() { }
}