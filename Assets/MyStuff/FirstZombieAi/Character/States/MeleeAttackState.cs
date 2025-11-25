using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : State
{
    public MeleeAttackState(Character _User) : base(_User)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        user.Animator.SetBool("IsAttackingMelee", true);
        user.Agent.isStopped = true;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (user.Target == null)
        {
            // Debug.LogWarning($"{user.gameObject.name} has no Enemy to Attack, but still is in the Melee Attack State!");

            user.IsEnemyInRange = false;
            return;
        }
        Vector3 LookAtTarget = user.Target.position;
        LookAtTarget.y = user.transform.position.y;

        user.transform.LookAt(LookAtTarget);

        user.SquareDistanceToEnemy = (user.transform.position - user.Target.position).sqrMagnitude;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        user.Agent.isStopped = false;
        user.Animator.SetBool("IsAttackingMelee", false);
    }
}
