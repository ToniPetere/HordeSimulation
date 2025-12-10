using System.Collections.Generic;
using UnityEngine;

public class ZombieStateMachine
{
    private State currentState;
    private Dictionary<State, List<Transition>> transitions;

    private Zombie owner;
    public Zombie Owner {  get { return owner; } }

    public State CurrentState { get { return currentState; } }

    public ZombieStateMachine(State _startState, Dictionary<State, List<Transition>> _transitions, Zombie _owner)
    {
        currentState = _startState;
        _startState.OnStateEnter();

        transitions = _transitions;
        owner = _owner;
    }

    
    private State GetNextState()
    {
        List<Transition> currentTransitions = transitions[currentState];

        foreach (Transition transition in currentTransitions)
        {
            if (transition.Condition()) return transition.TargetState;
        }

        return null;
    }

    public void ChangeState(State _targetState)
    {
        if (currentState == _targetState) return;

        currentState.OnStateExit();
        currentState = _targetState;
        currentState.OnStateEnter();
    }

    public void Tick() // Behaviour driven: 
    {
        if(owner.ControllType != EZombieControllType.BehaviourDriven)
            return;

        //Check if the State should be changed
        State nextState = GetNextState();
        if (nextState != null) ChangeState(nextState);

        //if not run the current Statelogic/-behaviour
        currentState.OnStateUpdate();


        // Debug.Log("Current State: " + CurrentState.ToString());
    }
}
