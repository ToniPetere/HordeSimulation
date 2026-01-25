using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ZombieView : MonoBehaviour
{
    public Entity Entity;
    public EntityManager EntityManager;

    private LocalTransform localTransform;


    void FixedUpdate()
    {
        // Does the according entity to this Visuals exist?
        if (!EntityManager.Exists(Entity))
        {
            //Note that this happens 1 Frame delayed to the entity death! With fixedUpdate maybe even more frames
            Destroy(this.gameObject);
            return;
        }

        // Sync Positions
        localTransform = EntityManager.GetComponentData<LocalTransform>(Entity); // localTransform is not a Refrence, it needs to be set every frame!
        transform.position = localTransform.Position;
        transform.rotation = localTransform.Rotation;
        Physics.SyncTransforms(); // This is an expensive Methode, that shouldnt be called in a Update Methode!!!!!!!!!!!! However its my only solution rn...
    }
}
