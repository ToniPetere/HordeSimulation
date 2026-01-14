using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class HordeAuthoring : MonoBehaviour
{
    public float timerMax;
    public float newWaypointRange;
    public float joinRadius;

    public class Baker : Baker<HordeAuthoring>
    {


        public override void Bake(HordeAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Horde
            {
                center = authoring.transform.position, // If the Horde Moves, the center also has to be synced! -> Create System for that if needed!
                timerMax = authoring.timerMax,
                newWaypointRange = authoring.newWaypointRange,
                joinRadiusSq = authoring.joinRadius * authoring.joinRadius
            });
        }
    }
}

public struct Horde : IComponentData
{
    // Have to be set
    public float3 center;
    public float newWaypointRange;
    public float joinRadiusSq;
    public float timerMax;

    // used for calculations
    public float timer;
}