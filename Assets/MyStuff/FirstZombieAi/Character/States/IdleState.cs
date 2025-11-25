using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(Character _User) : base(_User)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        user.Agent.isStopped = true; // Deaktivieren, des Agents, damit der Character sich nicht mehr Bewegt
        user.Animator.SetBool("IsIdle", true);


        user.IdleTime = GenerateIdleTime(user.MinIdleTime, user.MaxIdleTime);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        user.IdleTime -= Time.deltaTime;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        user.Animator.SetBool("IsIdle", false);
    }
    private float GenerateIdleTime(float _minTime, float _maxTime)
    {
        return Random.Range(_minTime, _maxTime);
    }
}
