using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct UnitTargetSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // Player Singleton lesen
        float3 playerPos = SystemAPI.GetSingleton<PlayerPosition>().Value;
        playerPos.y = 0; // set this to 0 Manually, so zombies dont fly -> Maybe change the ZombieMover System at some point...

        float chaseRangeSq = 10f * 10f;

        foreach ((
            RefRO<LocalTransform> transform,
            RefRW<UnitMover> mover
        ) in SystemAPI.Query<
            RefRO<LocalTransform>,
            RefRW<UnitMover>>())
        {
            float distSq = math.distancesq(transform.ValueRO.Position, playerPos);

            if (distSq <= chaseRangeSq)
            {
                mover.ValueRW.targetPosition = playerPos;
            }
        }
    }
}
