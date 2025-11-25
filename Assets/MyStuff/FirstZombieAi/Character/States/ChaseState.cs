using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public ChaseState(Character _User) : base(_User)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //user.Agent.speed *= user.SprintMultiplier;

        user.Agent.isStopped = false;
        user.Animator.SetBool("IsWalking", true);
    }
    
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        if (user.Target == null)
        {
            // Debug.LogWarning($"{user.gameObject.name} has no Enemy to Chase, but still is in the Chase State!");

            user.IsEnemyInRange = false;
            return;
        }

        // Chase the Enemy:
        user.WalkPoint = user.Target.position;
        user.Agent.SetDestination(user.WalkPoint);
        
        user.SquareDistanceToEnemy = (user.transform.position - user.Target.position).sqrMagnitude;

        //if (user.SquareDistanceToEnemy < 1f)
        //{
        //    Debug.Log("Walkpoint Reached!");
        //    user.HasWalkPoint = false;
        //}
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        user.Animator.SetBool("IsWalking", false);
        user.Agent.isStopped = true;

        //user.Agent.speed /= user.SprintMultiplier;
    }
}
