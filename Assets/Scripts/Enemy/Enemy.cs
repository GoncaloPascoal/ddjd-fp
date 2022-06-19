using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyCreated = delegate { };

    private Damageable _damageable;
    
    
    [SerializeField] private int EnemyMaxHealth = 40;

    protected bool mindControlled;
    [SerializeField] protected float viewDistance = 5f;
    [SerializeField] protected float fieldOfView = 70f;
    protected float initialFOV;

    [SerializeField] protected float chaseSpeed = 3f;
    [SerializeField] protected float walkSpeed = 1.5f;
    [SerializeField] protected float chaseCooldown = 5f;
    
    [SerializeField] protected Transform _spawn;
    protected Quaternion InitialOrientation;

    protected Transform PlayerTransform;
    protected ThirdPersonController playerTPC;
    
    protected NavMeshAgent NavMeshAgent;

    protected float AnimationBlend;
    public bool backstabbed { get; private set; }
    public float speedChangeRate = 10.0f;

    protected Animator _animator;

    [SerializeField] private List<string> targetsWhileMindControlled = new List<string>{"Enemy"};
    [SerializeField] private List<string> normalTargets = new List<string> {"Player", "MindControlled"};

    protected void Start()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.InitializeMaxHealth(EnemyMaxHealth);

        NavMeshAgent = GetComponent<NavMeshAgent>();
        PlayerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerTPC = PlayerTransform.GetComponent<ThirdPersonController>();

        _spawn.position = transform.position;
        InitialOrientation = transform.rotation;
        mindControlled = false;
        backstabbed = false;
        initialFOV = fieldOfView;

        _animator = GetComponent<Animator>();
        
        OnEnemyCreated.Invoke(this);
    }

    protected void Update()
    {
        AnimationBlend = Mathf.Lerp(AnimationBlend, NavMeshAgent.speed, Time.deltaTime * speedChangeRate);
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
            {
                return normalTargets.Contains(hit.transform.tag);
            }

            return targetsWhileMindControlled.Contains(hit.transform.tag);
        }
        
        return false;
    }

    protected Vector3 GetTargetPos()
    {
        var headPos = playerTPC.playerHeadTransform.position;
        var playerPos = 
            new Vector3(headPos.x, headPos.y - 0.5f,
                headPos.z); // TODO: If the y position is too high, enemy aggro messes up and if the player stays still and a melee enemy gets near, the melee enemy slides inside the player
        
        if (!mindControlled)
        {
            return GetClosestWithTags(normalTargets).position + new Vector3(0, 0.5f, 0);
        }
        var closestEnemy = GetClosestWithTags(targetsWhileMindControlled);

        if (closestEnemy != null)
        {
            if (targetsWhileMindControlled.Contains("Player"))
                targetsWhileMindControlled.Remove("Player");
            
            return closestEnemy.position + new Vector3(0, 0.5f, 0);
        }

        // if no enemy is found the enemy attacks the player instead - TODO: what should we do in this case?
        targetsWhileMindControlled.Add("Player");
        return playerPos;
    }

    Transform GetClosestWithTags(List<String> tags)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (string tag in tags)
        {
            foreach(GameObject potentialTarget in GameObject.FindGameObjectsWithTag(tag))
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
        }
        
        return bestTarget;
    }

    public void SetFOV(float fov)
    {
        fieldOfView = fov;
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

    public void Backstab(int damage)
    {
        backstabbed = true;
        _animator.SetBool("Backstab", true);
        _damageable.ChangeHealth(-damage);
    }

    public void EndBackstab()
    {
        _animator.SetBool("Backstab", false);
        backstabbed = false;
    }
    
    public void MindControl()
    {
        mindControlled = true;
        gameObject.tag = "MindControlled";
        ChangeTargetsMindControl(targetsWhileMindControlled);
    }

    protected abstract void ChangeTargetsMindControl(List<string> newTargets);

    public void SetupHealthBar(Canvas canvas)
    {
        GameObject healthBar = _damageable.healthBar.gameObject;

        healthBar.transform.SetParent(canvas.transform);
        healthBar.AddComponent<FaceCamera>().targetCamera = Camera.main;
        var follow = healthBar.AddComponent<FollowTarget>();
        follow.target = transform;
        follow.offset = Vector3.up * 1.9f; // TODO: use enemy height to determine health bar position
    }
}
