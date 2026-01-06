using Unity.Entities;

partial struct PlayerProxyBootstrapSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.EntityManager.CreateEntity(typeof(PlayerPosition));
    }
}