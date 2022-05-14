using Cinemachine.Editor;
using UnityEngine;

public class EnemyMelee : Enemy
{
    private bool _chasingPlayer;
    private float _chasingTime;
    private bool nearPlayer;

    [SerializeField] private float meleeDistance = 2f;

    [SerializeField]
    private float
        unconditionalDetectionRange = 1.5f; // enemy will attack player at this distance whether they are alert or not

    [SerializeField] private float alertRange = 5f;

    new void Start()
    {
        base.Start();
        _chasingPlayer = false;
        _chasingTime = chaseCooldown;
    }

    new void Update()
    {
        var detectingPlayer = DetectPlayer();

        var distanceToPlayer = Vector3.Distance(transform.position, GetPlayerPos());

        // if near player (attack range)
        if (distanceToPlayer <= meleeDistance)
        {
            // is chasing - will look at player to attack, or player is too close
            if (_chasingPlayer || distanceToPlayer <= unconditionalDetectionRange)
            {
                NavMeshAgent.speed = 0f;
                LookAtPlayer();
                base.Update();
                return;
            }
            // else, player is standing behind unsuspecting enemy
        }

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
            if (distanceToPlayer <= alertRange || !((_chasingTime -= Time.deltaTime) <= 0))
            {
                ChasePlayer();
            }
            else
            {
                _chasingPlayer = false;
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
            }
        }

        base.Update();
    }

    protected void ChasePlayer()
    {
        Vector3 targetVector = GetPlayerPos();
        NavMeshAgent.SetDestination(targetVector);
        NavMeshAgent.speed = chaseSpeed;
        _chasingPlayer = true;
    }

    protected void GoToSpawn()
    {
        Vector3 targetVector = _spawn.position;

        NavMeshAgent.SetDestination(targetVector);
        NavMeshAgent.speed = walkSpeed;
    }
}