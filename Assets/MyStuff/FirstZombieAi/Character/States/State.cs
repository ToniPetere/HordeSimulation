public abstract class State
{
    protected Character user;

    public State(Character _User)
    {
        user = _User;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }
}
