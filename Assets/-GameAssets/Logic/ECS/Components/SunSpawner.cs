using Unity.Entities;
using Unity.Mathematics;

public struct SunSpawner : IComponentData {
    public Entity prefabSun;
    public float3 sunPosition;
    public float sunMass;
}
