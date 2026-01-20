using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MeleeAttackSystem : ISystem
{
    // My System: Zombie(Entity) <-> Player(OOP)
    // [BurstCompile] -> cant be activated, because I acces a Singleton(a static reference)
    public void OnUpdate(ref SystemState state)
    {
        // Player Singleton lesen
        float3 playerPos = SystemAPI.GetSingleton<PlayerPosition>().Value;
        PlayerHealth playerHealth = PlayerHealth.Instance;
        playerPos.y = 0; // set this to 0 Manually, so zombies ignore height -> Maybe change this at some point

        int totalDamageThisFrame = 0;

        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRW<MeleeAttack> meleeAttack,
            RefRW<UnitMover> unitMover)
            in SystemAPI.Query<
            RefRO<LocalTransform>,
            RefRW<MeleeAttack>,
            RefRW<UnitMover>
            >())
        {
            float attackRangeSq = meleeAttack.ValueRO.attackRange * meleeAttack.ValueRO.attackRange;

            float distSq = math.distancesq(localTransform.ValueRO.Position, playerPos); // Check distance to the player
            if (distSq <= attackRangeSq) // If the player is close enough, attack him
            {
                // Count down the Timer
                meleeAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                if (meleeAttack.ValueRO.timer > 0)
                {
                    continue;
                }
                meleeAttack.ValueRW.timer = meleeAttack.ValueRO.timerMax;

                //Calculate Damage the Player takes:
                totalDamageThisFrame += meleeAttack.ValueRO.damageAmount;
            }
        }
        if(totalDamageThisFrame > 0)
        {
            // Apply damage to the player at the end
            PlayerHealth.Instance.TakeDamage(totalDamageThisFrame); // This blocks the BurstCompiler! 
        }
    }
    // -> Could refactor it for BurstCompilation:
    //
    // Just store the damage for now in a struct:
    // public struct PlayerDamageThisFrame : IComponentData
    // {
    //     public int value;
    // }

    // Apply damage in another System:
    // partial class ApplyPlayerDamageSystem : SystemBase
    // {
    // protected override void OnUpdate()
    // {
    //     var damage = SystemAPI.GetSingletonRW<PlayerDamageThisFrame>();
          
    //     if (damage.ValueRO.value > 0)
    //     {
    //         PlayerHealth.Instance.TakeDamage(damage.ValueRO.value);
    //         damage.ValueRW.value = 0;
    //     }
    // }


    // --------------------------------------------------------------------------


    //From the Lecture: Zombie(Entity) <-> Units(Entity)
    /*
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRW<MeleeAttack> meleeAttack,
            RefRO<Target> target,
            RefRW<UnitMover> unitMover)
            in SystemAPI.Query<
            RefRO<LocalTransform>,
            RefRW<MeleeAttack>,
            RefRO<Target>,
            RefRW<UnitMover>
            >())
        {
            if(target.ValueRO.targetEntity == Entity.Null)
            {
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            float meleeAttackDistanceSq = 2f;
            bool isCloseEnoughToAttack = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) < meleeAttackDistanceSq)

            if(!isCloseEnoughToAttack) 
            {
                // Target is too far
                unitMover.ValueRW.targetPosition = targetLocalTransform.Position; // Move closer
            }
            else
            {
                // Target is close Enough to Attack
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position; // Stop moving

                // Count down the Timer
                meleeAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                if(meleeAttack.ValueRO.timer > 0 )
                {
                    continue;
                }
                meleeAttack.ValueRW.timer = meleeAttack.ValueRO.timerMax;

                // If Timer is counted down, perform Damage
                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.healthAmount -= meleeAttack.ValueRO.damageAmount;

                //Maybe add a event to update UI Healthbars
            }
        }
    }
    */
}
