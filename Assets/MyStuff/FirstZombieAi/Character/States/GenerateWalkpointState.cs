using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWalkpointState : State
{
    Zombie zombieUser;
    public GenerateWalkpointState(Character _User) : base(_User)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        zombieUser = (Zombie)user;

        if (!zombieUser.HasWalkPoint)
        {
            zombieUser.WalkPoint = GenerateNewWalkpoint(zombieUser.WalkpointRange);
            zombieUser.HasWalkPoint = true;
        }
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    private Vector3 GenerateNewWalkpoint(float _range)
    {
        float randomX = Random.Range(-_range, _range);
        float randomZ = Random.Range(-_range, _range);

        return new Vector3(randomX, 0f, randomZ);
    }
}
