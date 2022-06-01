using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class Enemy : MonoBehaviour
{
    protected bool mindControlled;
    [SerializeField] protected float viewDistance = 5f;
    [SerializeField] protected float fieldOfView = 70f;

    [SerializeField] protected float chaseSpeed = 3f;
    [SerializeField] protected float walkSpeed = 1.5f;
    [SerializeField] protected float chaseCooldown = 5f;
    
    [SerializeField] protected Transform _spawn;
    protected Quaternion InitialOrientation;
    
    protected Transform PlayerTransform;
    protected ThirdPersonController playerTPC;

    protected NavMeshAgent NavMeshAgent;
    
    // animation IDs
    protected int AnimIDSpeed;
    protected int AnimIDMotionSpeed;

    protected Animator Animator;
    protected bool HasAnimator;

    protected float AnimationBlend;
    public float speedChangeRate = 10.0f;

    // Start is called before the first frame update
    public void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        PlayerTransform = GameObject.Find("PlayerArmature").GetComponent<Transform>();
        playerTPC = PlayerTransform.GetComponent<ThirdPersonController>();
        HasAnimator = TryGetComponent(out Animator);

        _spawn.position = transform.position;
        InitialOrientation = transform.rotation;
        mindControlled = false;

        AssignAnimationIDs();

        if (HasAnimator)
        {
            Animator.SetFloat(AnimIDMotionSpeed, 1f);
        }
        
    }

    // Update is called once per frame
    public void Update()
    {
        AnimationBlend = Mathf.Lerp(AnimationBlend, NavMeshAgent.speed, Time.deltaTime * speedChangeRate);
        Animator.SetFloat(AnimIDSpeed, AnimationBlend);
    }
    
    private void AssignAnimationIDs()
    {
        AnimIDSpeed = Animator.StringToHash("Speed");
        AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    protected bool DetectTarget()
    {
        Vector3 rayDirection = GetTargetPos() - transform.position;
        
        // if target is within view distance
        if (Vector3.Magnitude(rayDirection) > viewDistance)
            return false;
        
        // target not inside field of view cone
        if (Vector3.Angle(transform.forward, new Vector3(rayDirection.x, 0f, rayDirection.z)) > fieldOfView / 2.0f) 
            return false;
        
        //  and no obstacles in the way
        if (Physics.Raycast(transform.position, rayDirection, out var hit, viewDistance))
        {
            if (!mindControlled)
                return hit.transform.CompareTag("Player");
            return hit.transform.CompareTag("Enemy");
        }
        
        return false;
    }

    Transform GetClosestEnemy ()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(GameObject.ReferenceEquals(this.gameObject, potentialTarget)) continue;
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget;
    }
    
    protected Vector3 GetTargetPos()
    {
        var headPos = playerTPC.playerHeadTransform.position;
        var playerPos = 
            new Vector3(headPos.x, headPos.y - 0.5f,
                headPos.z); // TODO: If the y position is too high, enemy aggro messes up and if the player stays still and a melee enemy gets near, the melee enemy slides inside the player
        
        if (!mindControlled)
        {
            return playerPos;
        }
        else
        {
            var closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
                return GetClosestEnemy().position + new Vector3(0, 0.5f, 0);

            // if no enemy is found the enemy attacks the player instead - TODO: what should we do in this case?
            mindControlled = false;
            return playerPos;
        }
    }

    protected void LookAtTarget()
    {
        Quaternion lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            Quaternion.LookRotation(GetTargetPos() - transform.position).eulerAngles.y,
            transform.rotation.eulerAngles.z);
        transform.rotation =
            Quaternion.Lerp(transform.rotation, lookRotation,
                Time.deltaTime * 5f); // TODO: change hardcoded lerp speed
    }

    public void mindControl()
    {
        this.mindControlled = true;
    }
}
