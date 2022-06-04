using UnityEngine;

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

    private Animator _animator;
    
    // animation IDs
    private int _animIDSpeed;
    private int _animIDMotionSpeed;

    new void Start()
    {
        base.Start();
        _attacker = GetComponent<Attacker>();
        _animator = GetComponent<Animator>();
        _chasingTarget = false;
        _chasingTime = chaseCooldown;
        
        AssignAnimationIDs();

        _animator.SetFloat(_animIDMotionSpeed, 1f);
    }

    new void Update()
    {
        var detectingTarget = DetectTarget();
        var targetPos = GetTargetPos();
        var position = transform.position;
        var distanceToTarget = Vector2.Distance(new Vector2(position.x,position.z), new Vector2(targetPos.x, targetPos.z));

        // if near target (attack range)
        if (distanceToTarget <= meleeDistance)
        {
            // is chasing - will look at player to attack, or player is too close
            
            NavMeshAgent.SetDestination(gameObject.transform.position);
            if (_chasingTarget || distanceToTarget <= unconditionalDetectionRange)
            {
                NavMeshAgent.speed = 0f;
                LookAtTarget();
                
                _attacker.AttackNotBuffered();
                
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
        
        _animator.SetFloat(_animIDSpeed, AnimationBlend);

        base.Update();
    }
    
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void ChaseTarget()
    {
        Vector3 targetVector = GetTargetPos();
        NavMeshAgent.SetDestination(targetVector);
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
}