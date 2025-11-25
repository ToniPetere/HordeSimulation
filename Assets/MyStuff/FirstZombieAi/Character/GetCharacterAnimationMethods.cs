using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCharacterAnimationMethods : MonoBehaviour
{
    private Character character;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
    }

    public void DoMeleeAttack(AnimationEvent animationEvent)
    {
        character.RunMeleeAttack();
        // character.SpawnMeleeAttack();
    }

}
