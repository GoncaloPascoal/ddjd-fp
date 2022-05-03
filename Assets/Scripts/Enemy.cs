using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private float viewDistance;
    [SerializeField] private float fieldOfView;

    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float chaseCooldown = 5f;
    
    [SerializeField] private Transform _spawn;
    private Quaternion _initialOrientation;
    
    private Transform _playerTransform;

    private NavMeshAgent _navMeshAgent;

    private bool _chasingPlayer = false;
    private float _chasingTime;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.Find("PlayerArmature").GetComponent<Transform>();

        _spawn.position = transform.position;
        _initialOrientation = transform.rotation.normalized; 
            
        _chasingPlayer = false;
        _chasingTime = chaseCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        bool detectingPlayer = DetectPlayer();
        
        if (detectingPlayer)
        {
            if (!_chasingPlayer)
            {
                _chasingTime = chaseCooldown;
            }
            
            ChasePlayer();
        } 
        else if (_chasingPlayer)
        {
            if ((_chasingTime -= Time.deltaTime) <= 0)
            {
                _chasingPlayer = false;
                GoToSpawn();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            // if near spawn, reset to initial orientation
            if (Vector3.Distance(transform.position, _spawn.position) <= 0.2)
            {
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, _initialOrientation.eulerAngles, Time.deltaTime));
            }
        }
    }

    bool DetectPlayer()
    {
        Vector3 rayDirection = GetPlayerPos() - transform.position;
        
        // if player is within view distance
        if (Vector3.Magnitude(rayDirection) > viewDistance)
            return false;
        
        // player not inside field of view cone
        if (Vector3.Angle(transform.forward, rayDirection) > fieldOfView / 2.0f) 
            return false;
        
        //  and no obstacles in the way
        if (Physics.Raycast(transform.position, rayDirection, out var hit, viewDistance)) 
        {
            return hit.transform.CompareTag("Player");
        }
        
        return false;
    }

    private void ChasePlayer()
    {
        Vector3 targetVector = GetPlayerPos(); 
        _navMeshAgent.SetDestination(targetVector);
        _navMeshAgent.speed = chaseSpeed;
        _chasingPlayer = true;
    }

    private void GoToSpawn()
    {
        Vector3 targetVector = _spawn.position;

        _navMeshAgent.SetDestination(targetVector);
        _navMeshAgent.speed = walkSpeed;
    }

    private Vector3 GetPlayerPos()
    {
        return _playerTransform.position + Vector3.up; // TODO: change hardcoded height offset to player pos
    }
}
