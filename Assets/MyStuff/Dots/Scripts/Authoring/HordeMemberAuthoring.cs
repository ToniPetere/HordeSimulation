using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class HordeMemberAuthoring : MonoBehaviour
{
    public class Baker : Baker<HordeMemberAuthoring>
    {
        public override void Bake(HordeMemberAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HordeMember
            {

            });
        }
    }
}

public struct HordeMember : IComponentData
{
     public Entity hordeEntity; 
}