using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class GenerateWalkpointState : State
{
    public GenerateWalkpointState(Zombie _User) : base(_User)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();


        if (!user.HasWalkPoint)
        {
            user.WalkPoint = GenerateNewWalkpoint(user.WalkpointRange);
            user.HasWalkPoint = true;
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
