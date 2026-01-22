using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

public class ZombieViewSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombieGOPrefab;

    EntityManager entityManager;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Update()
    {
        // Get all Entities with the NewZombieTag (and LocalTransform)
        EntityQuery entityQuery = entityManager.CreateEntityQuery(
            typeof(NewZombieTag),
            typeof(LocalTransform)
        );

        // Create an Array out of this zombie entities
        NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);

        // Create an ZombieGO for each zombie Entity. GameObjects hold the Visuals(+Animator) and are neccessary for Physic calculations!
        foreach (Entity entity in entities)
        {
            LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(entity);

            GameObject zombieGO = Instantiate(zombieGOPrefab, localTransform.Position, Quaternion.identity);

            ZombieView zombieView = zombieGO.GetComponent<ZombieView>(); // The prefab has to have the ZombieView Script!
            zombieView.Entity = entity;
            zombieView.EntityManager = entityManager;

            // Remove the Tag, as it is only a marker to check, what Zombie still needs an according GO
            entityManager.RemoveComponent<NewZombieTag>(entity);
        }

        // I think: Free up the memory of the array, as it is not needed/used anymore
        entities.Dispose();
    }
}
