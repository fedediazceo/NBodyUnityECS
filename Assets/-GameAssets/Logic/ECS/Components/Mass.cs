using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Mass : IComponentData {
    public float Value;
}
