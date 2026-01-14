using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ZombieJoinsHordeSystem : ISystem
{
    // Vollkommen selber geschrieben: 

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        foreach ((
            RefRW<HordeMember> hordeMember,
            RefRO<LocalTransform> localTransform)
            in SystemAPI.Query<
                RefRW<HordeMember>,
                RefRO<LocalTransform>
                >())
        {
            if(hordeMember.ValueRO.hordeEntity != Entity.Null) // If the Zombie already belongs to a horde, skip him
            {
                continue;
            }

            foreach ((
                RefRO<Horde> horde,
                Entity hordeEntity)
                in SystemAPI.Query<
                RefRO<Horde>
                >().WithEntityAccess())
            {
                // if zombie is in Range of the Horde, then join the Horde
                if (math.distancesq(localTransform.ValueRO.Position, horde.ValueRO.center) <= horde.ValueRO.joinRadiusSq) // Distance from the the Center to Zombie = Distance from the Horde to the Zombie
                {
                    hordeMember.ValueRW.hordeEntity = hordeEntity;
                    break;
                }
            }
        }
        //Feedback:
        //Jeder Zombie prüft jede Horde!
        //Bei 500 Zombies und 20 Horden = 10.000 Checks / Frame
        // -> vllt nochmal überarbeiten
    }
}
