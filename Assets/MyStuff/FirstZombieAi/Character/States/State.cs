public abstract class State
{
    protected Zombie user;

    public State(Zombie _User)
    {
        user = _User;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }
}
