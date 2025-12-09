using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    private List<Zombie> zombies;
    [SerializeField] private List<ZombieStateMachine> stateMachines = new List<ZombieStateMachine>();


    private void Start()
    {
        zombies = ZombieList.Instance.Zombies;
        // Debug.LogWarning("zombies Count: " + zombies.Count);

        //foreach (var zombie in zombies)
        //    zombie.OnControlTypeChanged += HandleControlTypeChanged; // not needed rn

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

        stateMachines.Add(new ZombieStateMachine(idleState, transitions, _zombie));
    }
    private IEnumerator InitializeStateMachines() // Old "Automatic" logic
    {
        yield return null;

        stateMachines = new List<ZombieStateMachine>();

        foreach (Zombie zombie in zombies)
        {
            CreateStateMachine(zombie);
        }
    }

    //!!! Is actually not needed. As the ControllType on its own already handles wether the StateMachine is Active or not!!!
    //private void HandleControlTypeChanged(Zombie _zombie, EZombieControllType _type)
    //{
    //    if (_type == EZombieControllType.BehaviourDriven)
    //        ToggleStateMachine(_zombie, true); // Activate its StateMachine if the ZombieBehaviour is set to BehaviourDriven

    //    if (_type == EZombieControllType.HordeDriven)
    //        ToggleStateMachine(_zombie, false);


    //    // In theory should never happen if everything works as intended
    //    if(_type == EZombieControllType.None)
    //        Debug.LogWarning(_zombie.name + " had an Invalid Controll Type! Expect him to be Buggy!!!");
    //}
    //private void ToggleStateMachine(Zombie _zombie, bool _setActive)
    //{
    //    if (_setActive)
    //    {

    //    }
    //}
}
