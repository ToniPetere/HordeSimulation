using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct HealthDeadTestSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //This will guarante, that this happens at the end of the frame, so no refrenze errors occur
        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRO<Health> health,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<Health>>().WithEntityAccess())
        {
            if(health.ValueRO.healthAmount <= 0)
            {
                // Entity is dead:
                // state.EntityManager.DestroyEntity(entity); -> This does not work that easily, will throw an error!!!
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
