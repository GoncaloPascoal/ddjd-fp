using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAttack : StateMachineBehaviour
{
    private AttackerBoss _attacker;

    [SerializeField] private float cooldown;
    [SerializeField] private float chanceOfEndingCombo;
    [SerializeField] private List<string> possibleFollowUps;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attacker = animator.GetComponent<AttackerBoss>();
        _attacker.AttackBoss();
        
        animator.SetFloat("Cooldown", cooldown);
        
        var willEnd= possibleFollowUps.Count == 0 || Random.Range(0f, 1f) <= chanceOfEndingCombo;

        // choose follow up
        if (!willEnd)
        {
            var followUp = Random.Range(0, possibleFollowUps.Count);
            animator.SetTrigger(possibleFollowUps[followUp]);
        }
        // else, combo will end here
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attacker.EndAttack();
    }
}
