using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class Enemy : MonoBehaviour
{
    
    [SerializeField] protected float viewDistance = 5f;
    [SerializeField] protected float fieldOfView = 70f;

    [SerializeField] protected float chaseSpeed = 3f;
    [SerializeField] protected float walkSpeed = 1.5f;
    [SerializeField] protected float chaseCooldown = 5f;
    
    [SerializeField] protected Transform _spawn;
    protected Quaternion InitialOrientation;
    
    protected Transform PlayerTransform;
    
    // get player bounds
    protected float playerMeshHeight;

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
        playerMeshHeight = PlayerTransform.gameObject.transform.Find("Geometry/Armature_Mesh").GetComponent<SkinnedMeshRenderer>().bounds.size.y;
        
        HasAnimator = TryGetComponent(out Animator);

        _spawn.position = transform.position;
        InitialOrientation = transform.rotation;

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

    protected bool DetectPlayer()
    {
        Vector3 rayDirection = GetPlayerPos() - transform.position;
        
        // if player is within view distance
        if (Vector3.Magnitude(rayDirection) > viewDistance)
            return false;
        
        // player not inside field of view cone
        if (Vector3.Angle(transform.forward, new Vector3(rayDirection.x, 0f, rayDirection.z)) > fieldOfView / 2.0f) 
            return false;
        
        //  and no obstacles in the way
        if (Physics.Raycast(transform.position, rayDirection, out var hit, viewDistance)) 
        {
            return hit.transform.CompareTag("Player");
        }
        
        return false;
    }

    protected Vector3 GetPlayerPos()
    {
        var playerPos = PlayerTransform.position;
        return new Vector3(playerPos.x, playerPos.y + playerMeshHeight, playerPos.z);
    }
}
