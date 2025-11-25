using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    private List<Zombie> zombies;
    private List<CharacterStateMachine> stateMachines = new List<CharacterStateMachine>();


    private void Start()
    {
        zombies = ZombieList.Instance.Zombies;

        // Debug.LogWarning("zombies Count: " + zombies.Count);

        // Use a Coroutine, because the Zombies gets added to the zombieList in the Start, so I need a delay to create the statemachines for them
        StartCoroutine(InitializeStateMachines());
    }
    void Update()
    {
        // Debug.Log("Zombie StateMachines: " + stateMachines.Count);
        for (int i = stateMachines.Count - 1; i >= 0; i--)
        {
            if (stateMachines[i].Owner == null)
            {
                stateMachines.Remove(stateMachines[i]);
                continue;
            }

            stateMachines[i].Tick();
        }
    }
    private void CreateStateMachine(Zombie _zombie)
    {
        IdleState idleState = new IdleState(_zombie);
        GenerateWalkpointState generateWalkpointState = new GenerateWalkpointState(_zombie);
        WalkState walkState = new WalkState(_zombie);
        ChaseState chaseState = new ChaseState(_zombie);
        MeleeAttackState meleeAttackState = new MeleeAttackState(_zombie);

        Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>()
        {
            [idleState] = new List<Transition>()
            {
                new Transition(chaseState, () => _zombie.IsEnemyInRange),
                new Transition(generateWalkpointState, () => _zombie.IdleTime <= 0)
            },

            [generateWalkpointState] = new List<Transition>()
            {
                new Transition(walkState, () => _zombie.HasWalkPoint)
            },

            [walkState] = new List<Transition>()
            {
                new Transition(chaseState, () => _zombie.IsEnemyInRange),
                new Transition(idleState, () => !_zombie.HasWalkPoint)
            },

            [chaseState] = new List<Transition>()
            {
                new Transition(meleeAttackState, () => _zombie.CheckForEnemyInMeleeRange()),
                new Transition(idleState, () => !_zombie.IsEnemyInRange)
            },

            [meleeAttackState] = new List<Transition>()
            {
                new Transition(chaseState, () => !_zombie.CheckForEnemyInMeleeRange()),
                new Transition(idleState, () => !_zombie.IsEnemyInRange)
            }
        };

        stateMachines.Add(new CharacterStateMachine(idleState, transitions, _zombie));
    }
    private IEnumerator InitializeStateMachines()
    {
        yield return null;

        stateMachines = new List<CharacterStateMachine>();

        foreach (Zombie zombie in zombies)
        {
            CreateStateMachine(zombie);
        }
    }
}
