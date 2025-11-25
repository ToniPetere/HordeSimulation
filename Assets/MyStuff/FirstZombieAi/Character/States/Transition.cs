using System;

public class Transition
{
    private Func<bool> condition;
    private State targetState;

    public Func<bool> Condition => condition;
    public State TargetState => targetState;

    public Transition(State _targetState, Func<bool> _condition) //Könnte man nicht auch einfach gleich einen Bool entgegen nehmen?
    {
        targetState = _targetState;
        condition = _condition;
    }
}
