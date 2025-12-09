using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
    public WalkState(Zombie _User) : base(_User)
    {
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        user.Agent.isStopped = false;
        user.Animator.SetBool("IsWalking", true);
        user.Agent.SetDestination(user.WalkPoint);
        //Debug.Log("Zombie now walks with FSM to a walkpoint!");
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if((user.transform.position - user.WalkPoint).sqrMagnitude < 1f)
        {
            //Debug.Log("Walkpoint Reached with FSM!");
            user.HasWalkPoint = false;
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        user.Animator.SetBool("IsWalking", false);
        user.HasWalkPoint = false;
    }
}
