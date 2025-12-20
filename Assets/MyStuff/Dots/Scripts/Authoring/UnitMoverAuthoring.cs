using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class UnitMoverAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public class Baker : Baker<UnitMoverAuthoring>
    {
        public override void Bake(UnitMoverAuthoring authoring) // This runs during the Bake process
        {
            //This creates an Entity wich has the Dots Transform Component
            Entity entity = GetEntity(TransformUsageFlags.Dynamic); // None, if there is nothing to be rendered(f.e. "Inventory". Renderable if there is something to be Rendered but it never Moves(a Static Mesh(f.e. a House). Dynamic if something has to be Rendered and it can Move("Characters")!
            AddComponent(entity, new UnitMover //This adds an Component to that entity(our custom "MoveSpeed" component in this example)
            {
                moveSpeed = authoring.moveSpeed, // Initialize the Component with the Value that we have in the Authoring Script set
                rotationSpeed = authoring.rotationSpeed,
            });
        }
    }
}

public struct UnitMover : IComponentData
{
    public float moveSpeed;
    public float rotationSpeed;
    public float3 targetPosition;
}

