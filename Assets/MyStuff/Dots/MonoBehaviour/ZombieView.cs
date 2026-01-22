using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ZombieView : MonoBehaviour
{
    public Entity Entity;
    public EntityManager EntityManager;

    private LocalTransform localTransform;


    void Update()
    {
        // Does the according entity to this Visuals exist?
        if (!EntityManager.Exists(Entity)) return;

        // Sync Positions
        localTransform = EntityManager.GetComponentData<LocalTransform>(Entity); // localTransform is not a Refrence, it needs to be set every frame!
        transform.position = localTransform.Position;
        transform.rotation = localTransform.Rotation;
    }
}
