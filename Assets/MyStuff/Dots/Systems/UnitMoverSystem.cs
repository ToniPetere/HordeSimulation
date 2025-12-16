using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

partial struct UnitMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (( // you can also put a var here in front to remove the VariableTypes in the next 3 lines(LocalTransform, MoveSpeed & PhysicsVelocity)
            RefRW<LocalTransform> localTransform,           //RefRW = ReadWrite, there is also RefRO = ReadOnly. Use RO when you can and RW when you need it. Only RO can be multithreaded
            RefRO<MoveSpeed> moveSpeed,                     
            RefRW<PhysicsVelocity> physicsVelocity)         // It will look for entities that have all 3 of the Components!      
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<MoveSpeed>,
                RefRW<PhysicsVelocity>>()
            ) 
        {
            float3 targetPosition = localTransform.ValueRO.Position + new float3(10, 0 ,0); // the point where to move to
            float3 moveDirection = targetPosition - localTransform.ValueRO.Position; // Direction Vector for the Unit
            moveDirection = math.normalize(moveDirection);                           // Direction Vector Normalized

            // Unit should look to the direction its moving
            localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up()); // math.up = new float3(0, 1, 0)
            physicsVelocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.value; // No deltaTime needed here, because Physics already makes it framerate independant
            physicsVelocity.ValueRW.Angular = float3.zero;
            // localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.value * SystemAPI.Time.DeltaTime; // float3 = Vector3. SystemAPI.Time.DeltaTime = Delta Time
        }
    }

}
