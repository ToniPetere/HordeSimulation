using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct HordeWaypointSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((
            RefRW<Horde> horde,
            Entity hordeEntity) // Entity brauche ich später um es nachzuprüfen, ob die Zombies zu dieser Horde gehören
            in SystemAPI.Query<
                RefRW<Horde>>().WithEntityAccess()) // WithEntityAccess gibt mir das Entity von der Horde
        {
            horde.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (horde.ValueRO.timer > 0f)
            {
                continue;
            }
            horde.ValueRW.timer = horde.ValueRO.timerMax;

            foreach((
                RefRO<HordeMember> hordeMember,
                RefRW<UnitMover> unitMover)
                in SystemAPI.Query<
                    RefRO<HordeMember>,
                    RefRW<UnitMover>>())
            {
                if(hordeMember.ValueRO.hordeEntity != hordeEntity) // Check if the Zombie belongs to this Horde
                {
                    continue;
                }

                if (unitMover.ValueRO.hasTarget) // If the zombie is already chasing something, ignore him (-> He doesnt need a random walkpoint then)
                {
                    continue;
                }

                // Center + Offset = Random Walkpoint around the HordeEntity
                float3 randomWalkpoint = horde.ValueRO.center + new float3( 
                    UnityEngine.Random.Range(-horde.ValueRO.newWaypointRange, horde.ValueRO.newWaypointRange),
                    0,
                    UnityEngine.Random.Range(-horde.ValueRO.newWaypointRange, horde.ValueRO.newWaypointRange)
                );

                unitMover.ValueRW.targetPosition = randomWalkpoint;
                unitMover.ValueRW.hasTarget = true;
            }
        }
    }
}
