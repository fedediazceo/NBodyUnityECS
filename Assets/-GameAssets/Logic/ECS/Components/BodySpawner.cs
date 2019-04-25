using Unity.Entities;

public struct BodySpawner : IComponentData
{
    public Entity bodyEntity;
    public int bodyCount;
    public float spaceWidth;
    public float spaceHeight;
    public float spaceDepth;
    public float bodyMass;
}
