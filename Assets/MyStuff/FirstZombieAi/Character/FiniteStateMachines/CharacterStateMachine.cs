using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine
{
    private State currentState;
    private Dictionary<State, List<Transition>> transitions;

    private Character owner;
    public Character Owner {  get { return owner; } }

    public State CurrentState { get { return currentState; } }

    public CharacterStateMachine(State _startState, Dictionary<State, List<Transition>> _transitions, Character _owner)
    {
        currentState = _startState;
        _startState.OnStateEnter();

        transitions = _transitions;
        owner = _owner;
    }

    //public void Initialize(State _startingState)
    //{
    //    currentState = _startingState;
    //    currentState.OnStateEnter();
    //}
    
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

    public void Tick()
    {
        State nextState = GetNextState();

        if (nextState != null) ChangeState(nextState);

        currentState.OnStateUpdate();

        // Debug.Log("Current State: " + CurrentState.ToString());
    }
}
