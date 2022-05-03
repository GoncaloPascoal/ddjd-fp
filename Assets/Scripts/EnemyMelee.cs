
using Cinemachine.Editor;
using UnityEngine;

public class EnemyMelee : Enemy
{
    private bool _chasingPlayer;
    private float _chasingTime;
    private bool nearPlayer;

    [SerializeField] private float meleeDistance = 2f;
    
    new void Start()
    {
        base.Start();
        _chasingPlayer = false;
        _chasingTime = chaseCooldown;
    }
    new void Update()
    {
        var detectingPlayer = DetectPlayer();

        // if near player (attack range)
        if (Vector3.Distance(transform.position, GetPlayerPos()) <= meleeDistance)
        {
            Debug.Log("near");
            _chasingPlayer = false;
            NavMeshAgent.speed = 0f;
            Quaternion lookRotation = Quaternion.LookRotation(GetPlayerPos() - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // TODO: change hardcoded lerp speed
            
            base.Update();
            return;
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
