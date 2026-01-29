using Unity.Entities;
using UnityEngine;

public class ZombieAnimationStateAuthoring : MonoBehaviour
{
    public EZombieAnimationState startingState;

    public class Baker : Baker<ZombieAnimationStateAuthoring>
    {
        public override void Bake(ZombieAnimationStateAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ZombieAnimationState
            {
                Value = authoring.startingState
            });
        }
    }
}

public struct ZombieAnimationState : IComponentData
{
    public EZombieAnimationState Value;
}
