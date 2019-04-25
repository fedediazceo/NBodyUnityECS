using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class VelocityProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var data = new Velocity { };
        dstManager.AddComponentData(entity, data);
    }
}
