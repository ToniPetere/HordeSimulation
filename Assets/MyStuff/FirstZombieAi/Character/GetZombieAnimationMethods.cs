using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetZombieAnimationMethods : MonoBehaviour
{
    private Zombie zombie;

    private void Awake()
    {
        zombie = GetComponentInParent<Zombie>();
    }

    public void DoMeleeAttack(AnimationEvent animationEvent)
    {
        zombie.RunMeleeAttack();
        // character.SpawnMeleeAttack();
    }

}
