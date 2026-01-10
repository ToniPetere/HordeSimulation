using Unity.Entities;
using UnityEngine;

public class EntitiesReferenceAuthoring : MonoBehaviour
{
    public GameObject zombiePrefabGameObject;

    public class Baker : Baker<EntitiesReferenceAuthoring> 
    {
        public override void Bake(EntitiesReferenceAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                zombiePrefabEntity = GetEntity(authoring.zombiePrefabGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}

partial struct EntitiesReferences : IComponentData
{
    public Entity zombiePrefabEntity;
}
