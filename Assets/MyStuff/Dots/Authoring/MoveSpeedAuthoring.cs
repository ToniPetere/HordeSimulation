using Unity.Entities;
using UnityEngine;

public class MoveSpeedAuthoring : MonoBehaviour
{
    public float value;

    public class Baker : Baker<MoveSpeedAuthoring>
    {
        public override void Bake(MoveSpeedAuthoring authoring) // This runs during the Bake process
        {
            //This creates an Entity wich has the Dots Transform Component
            Entity entity = GetEntity(TransformUsageFlags.Dynamic); // None, if there is nothing to be rendered(f.e. "Inventory". Renderable if there is something to be Rendered but it never Moves(a Static Mesh(f.e. a House). Dynamic if something has to be Rendered and it can Move("Characters")!
            AddComponent(entity, new MoveSpeed //This adds an Component to that entity(our custom "MoveSpeed" component in this example)
            {
                value = authoring.value, // Initialize the Component with the Value that we have in the Authoring Script set
            });
        }
    }
}

public struct MoveSpeed : IComponentData
{
    public float value;
}

