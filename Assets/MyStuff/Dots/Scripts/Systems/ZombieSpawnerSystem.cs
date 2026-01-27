using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;


[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct ZombieSpawnerSystem : ISystem
{
    // ChatGPT Fix:
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<EntitiesReferences>();
    }
    // ----------------------------------------------


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRW<ZombieSpawner> zombieSpawner)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<ZombieSpawner>
                >())
        {
            if(zombieSpawner.ValueRO.zombiesToSpawn <= 0) continue; // Stop when all zombies are spawned


            // Condition when to spawn a Zombie: (currently a timer)
            zombieSpawner.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (zombieSpawner.ValueRO.timer > 0f)
            {
                continue;
            }
            zombieSpawner.ValueRW.timer = zombieSpawner.ValueRO.timerMax;
            // zombieSpawner.ValueRW.timer = float.MaxValue; // Debug, to deaktivate the spawner after one spawn
            --zombieSpawner.ValueRW.zombiesToSpawn;


            //Spawn Zombie(without ecb -> Throws errors):
            //Entity zombieEntity = state.EntityManager.Instantiate(entitiesReferences.zombiePrefabEntity); // Why this works without a ecb I dont know
            //SystemAPI.SetComponent(zombieEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position)); // This seems to work without a ecb, propably because something existing just gets modified

            //Spawn Zombie via ecb:
            Entity zombieEntity = entityCommandBuffer.Instantiate(entitiesReferences.zombiePrefabEntity); //This throws an error though. Something like: "Playback() is missing"
            entityCommandBuffer.SetComponent(zombieEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));

            // Not allowed: state.EntityManager.AddComponent<NewZombieTag>(zombieEntity); 
            // You have to use a CommandBuffer:
            entityCommandBuffer.AddComponent<NewZombieTag>(zombieEntity); // Add this tag, to mark the Zombie to also instantiate an according GO for the Visuals/Physics
        }

    }
}
