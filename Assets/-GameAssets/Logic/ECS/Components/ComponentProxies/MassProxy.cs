using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public class MassProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var data = new Mass { };
        dstManager.AddComponentData(entity, data);
    }
}
