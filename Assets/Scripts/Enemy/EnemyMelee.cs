using UnityEngine;

public class EnemyMelee : Enemy
{
    private bool _chasingTarget;
    private float _chasingTime;
    private bool nearPlayer;

    [SerializeField] private float meleeDistance = 2f;

    [SerializeField]
    private float
        unconditionalDetectionRange = 1.0f; // enemy will attack player at this distance whether they are alert or not

    [SerializeField] private float alertRange = 5f;

    [SerializeField] private Attacker attacker;

    new void Start()
    {
        base.Start();
        attacker = GetComponent<Attacker>();
        _chasingTarget = false;
        _chasingTime = chaseCooldown;
    }

    new void Update()
    {
        var detectingTarget = DetectTarget();

        var distanceToTarget = Vector3.Distance(transform.position, GetTargetPos());

        // if near target (attack range)
        if (distanceToTarget <= meleeDistance)
        {
            // is chasing - will look at player to attack, or player is too close

            if (_chasingTarget || distanceToTarget <= unconditionalDetectionRange)
            {
                NavMeshAgent.speed = 0f;
                LookAtTarget();
                
                attacker.AttackNotBuffered();
                
                base.Update();
                return;
            }
            // else, player is standing behind unsuspecting enemy
        }

        if (detectingTarget)
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
            else
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
            }
        }

        base.Update();
    }

    protected void ChaseTarget()
    {
        Vector3 targetVector = GetTargetPos();
        NavMeshAgent.SetDestination(targetVector);
        NavMeshAgent.speed = chaseSpeed;
        _chasingTarget = true;
    }

    protected void GoToSpawn()
    {
        Vector3 targetVector = _spawn.position;

        NavMeshAgent.SetDestination(targetVector);
        NavMeshAgent.speed = walkSpeed;
    }
    
    private void OnDrawGizmos()
    {
        var iconPos = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        Gizmos.DrawIcon(iconPos, "Meelee.png", true);
    }
}