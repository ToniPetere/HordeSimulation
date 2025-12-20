using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        //*
        // Multithreaded with Jobs:
        UnitMoverJob unitMoverJob = new UnitMoverJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        };
        unitMoverJob.ScheduleParallel();
        //*/

        /*
        // Not Multithreaded!
        foreach (( // you can also put a var here in front to remove the VariableTypes in the next 3 lines(LocalTransform, MoveSpeed & PhysicsVelocity)
            RefRW<LocalTransform> localTransform,           //RefRW = ReadWrite, there is also RefRO = ReadOnly. Use RO when you can and RW when you need it. Only RO can be multithreaded
            RefRO<UnitMover> unitMover,
            RefRW<PhysicsVelocity> physicsVelocity)         // It will look for entities that have all 3 of the Components!      
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<UnitMover>,
                RefRW<PhysicsVelocity>>()
            )
        {
            float3 moveDirection = unitMover.ValueRO.targetPosition - localTransform.ValueRO.Position; // Direction Vector for the Unit

            float reachedTargetDistanceSqr = 4f;
            if (math.lengthsq(moveDirection) < reachedTargetDistanceSqr)
            {
                physicsVelocity.ValueRW.Linear = float3.zero; 
                physicsVelocity.ValueRW.Angular = float3.zero;
                return;
            }
            else
            {
                moveDirection = math.normalize(moveDirection);         // Direction Vector Normalized

                // Unit should look to the direction its moving
                localTransform.ValueRW.Rotation =
                    math.slerp(localTransform.ValueRO.Rotation,
                        quaternion.LookRotation(moveDirection, math.up()), // math.up = new float3(0, 1, 0)
                        SystemAPI.Time.DeltaTime * unitMover.ValueRO.rotationSpeed);

                physicsVelocity.ValueRW.Linear = moveDirection * unitMover.ValueRO.moveSpeed; // No deltaTime needed here, because Physics already makes it framerate independant
                physicsVelocity.ValueRW.Angular = float3.zero;
            }
        }
        */

    }

}

[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform _localTransform, in UnitMover _unitMover, ref PhysicsVelocity _physicsVelocity) // ref = ReadWrite, in = ReadOnly
    {
        float3 moveDirection = _unitMover.targetPosition - _localTransform.Position; // Direction Vector for the Unit
        float reachedTargetDistanceSqr = 4f;
        if(math.lengthsq(moveDirection) < reachedTargetDistanceSqr)
        {
            _physicsVelocity.Linear = float3.zero; // No deltaTime needed here, because Physics already makes it framerate independant
            _physicsVelocity.Angular = float3.zero;
            return;
        }

        moveDirection = math.normalize(moveDirection);                           // Direction Vector Normalized

        // Unit should look to the direction its moving
        _localTransform.Rotation = 
            math.slerp(_localTransform.Rotation, 
                quaternion.LookRotation(moveDirection, math.up()), // math.up = new float3(0, 1, 0)
                deltaTime * _unitMover.rotationSpeed); 

        _physicsVelocity.Linear = moveDirection * _unitMover.moveSpeed; // No deltaTime needed here, because Physics already makes it framerate independant
        _physicsVelocity.Angular = float3.zero;
    }
}