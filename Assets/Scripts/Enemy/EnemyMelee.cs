using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    private bool _chasingTarget;
    private float _chasingTime;

    [SerializeField] private float meleeDistance = 2f;

    [SerializeField]
    private float
        unconditionalDetectionRange = 1.0f; // enemy will attack player at this distance whether they are alert or not

    [SerializeField] private float alertRange = 5f;

    private Attacker _attacker;
    
    private float _attackCooldown = 0.0f;


    // animation IDs
    private int _animIDSpeed;
    private int _animIDMotionSpeed;

    private new void Start()
    {
        base.Start();
        _attacker = GetComponent<Attacker>();
        _chasingTarget = false;
        _chasingTime = chaseCooldown;
        
        AssignAnimationIDs();

        _animator.SetFloat(_animIDMotionSpeed, 1f);
    }

    private bool CanChase(float distanceToTarget)
    {
        return _chasingTarget || distanceToTarget <= unconditionalDetectionRange || fieldOfView >= 360f;
    }

    private new void Update()
    {
        if (backstabbed || gameObject.CompareTag("Dead")) return;

        bool detectedTarget = DetectTarget();
        Vector3? targetPosNullable = GetTargetPos();
        Vector3 position = transform.position;

        _attackCooldown -= Time.deltaTime;

        if (targetPosNullable != null)
        {
            Vector3 targetPos = (Vector3) targetPosNullable;
            float distanceToTarget = Vector2.Distance(new Vector2(position.x,position.z), new Vector2(targetPos.x, targetPos.z));

            // if near target (attack range)
            if (distanceToTarget <= meleeDistance)
            {
                // NavMeshAgent.SetDestination(gameObject.transform.position);
                StopMovement();

                // is chasing - will look at player to attack, or player is too close
                if (CanChase(distanceToTarget))
                {
                    if (!_attacker.IsAttacking()) LookAtTarget();

                    if (_attackCooldown <= 0)
                    {
                        _attacker.AttackNotBuffered();
                        _attackCooldown = 3f;
                    }
                    
                    _animator.SetFloat(_animIDSpeed, 0);

                    base.Update();
                    return;
                }
                // else, player is standing behind unsuspecting enemy
            }
            else if (!_attacker.IsAttacking())
            {
                if (detectedTarget || distanceToTarget <= unconditionalDetectionRange)
                {
                    if (!_chasingTarget)
                    {
                        _chasingTime = chaseCooldown;
                    }

                    ChaseTarget();
                }
                else if (_chasingTarget)
                {
                    if (distanceToTarget <= alertRange || !((_chasingTime -= Time.deltaTime) <= 0))
                    {
                        ChaseTarget();
                    }
                    else if (!gameObject.CompareTag("MindControlled"))
                    {
                        _chasingTarget = false;
                        GoToSpawn();
                    }
                }
                else
                {
                    // if near spawn, reset to initial orientation
                    if (Vector3.Distance(transform.position, _spawn.position) <= 0.2)
                    {
                        NavMeshAgent.speed = 0f;
                        transform.rotation = Quaternion.Lerp(transform.rotation, InitialOrientation, Time.deltaTime);
                        SetFOV(initialFOV);
                    }
                }
            }
        }
        
        _animator.SetFloat(_animIDSpeed, AnimationBlend);
        
        base.Update();
    }

    protected override void ChangeTargetsMindControl(List<string> newTargets)
    {
        _attacker.SetTargets(newTargets);
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void ChaseTarget()
    {
        Vector3? targetPos = GetTargetPos();
        if (targetPos == null)
        {
            StopMovement();
            return;
        }

        NavMeshAgent.SetDestination((Vector3) targetPos);
        NavMeshAgent.speed = chaseSpeed;
        _chasingTarget = true;
    }

    private void GoToSpawn()
    {
        Vector3 targetVector = _spawn.position;

        NavMeshAgent.SetDestination(targetVector);
        NavMeshAgent.speed = walkSpeed;
    }
    
    private void OnDrawGizmos()
    {
        var position = transform.position;
        var iconPos = new Vector3(position.x, position.y + 2.5f, position.z);
        Gizmos.DrawIcon(iconPos, "Meelee.png", true);
    }

    public override void StopMovement()
    {
        NavMeshAgent.isStopped = true;
        NavMeshAgent.ResetPath();
        NavMeshAgent.speed = 0f;
        _animator.SetFloat(_animIDSpeed, 0);
    }
    
}