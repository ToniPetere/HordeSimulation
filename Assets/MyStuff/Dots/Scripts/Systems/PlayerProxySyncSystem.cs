using Unity.Entities;
using UnityEngine;


// Anscheinen wichtig für später damit die Reihenfolge klar ist und Bugs verhindert werden. Andernfalls ist die Playerposition nicht 100% aktuell?
// Werde es auskommentieren, sollte es nötig sein
// [UpdateInGroup(typeof(SimulationSystemGroup))]
// [UpdateBefore(typeof(FindTargetSystem))]
partial struct PlayerProxySyncSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        Transform playerTransform = PlayerProxyAuthoring.Instance.transform;

        RefRW<PlayerPosition> proxy = SystemAPI.GetSingletonRW<PlayerPosition>();
        proxy.ValueRW.Value = playerTransform.position;
    }
}