using Unity.Burst;
using Unity.Entities;

partial struct ApplyDamageSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<Health> health,
            RefRO<PendingDamage> pendingDamage,
            Entity entity
            )
            in SystemAPI.Query<
                RefRW<Health>,
                RefRO<PendingDamage>
                >().WithEntityAccess())
        {
            health.ValueRW.healthAmount -= pendingDamage.ValueRO.value;

            entityCommandBuffer.RemoveComponent<PendingDamage>(entity);
        }
    }
}
