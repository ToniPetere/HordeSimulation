using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct FindTargetSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);

        foreach((RefRO<LocalTransform> localTransform,
                RefRO<FindTarget> findTarget)
                in SystemAPI.Query<
                    RefRO<LocalTransform>, 
                    RefRO<FindTarget>>())
        {
            distanceHitList.Clear();
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0
            };

            Debug.Log("-");
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, findTarget.ValueRO.range, ref distanceHitList, collisionFilter))
            {
                foreach(DistanceHit distanceHit in distanceHitList)
                {
                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if(targetUnit.faction == findTarget.ValueRO.targetFaction)
                    {
                        // Debug.Log(distanceHit.Entity); -> Thows an error?
                        // Valid Target!
                    }
                }
            }
        }


    }
}
