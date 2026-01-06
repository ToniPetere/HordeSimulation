using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerProxyAuthoring : MonoBehaviour
{
    // Singleton Strukture:
    public static PlayerProxyAuthoring Instance
    {
        get { return instance; }
        private set { instance = value; }
    }
    private static PlayerProxyAuthoring instance;
    private void Awake()
    {
        instance = this;
    }
}


public struct PlayerPosition : IComponentData
{
    public float3 Value;
}