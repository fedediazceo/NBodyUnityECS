using Unity.Entities;
using UnityEngine;

public class SunMassProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public float SunMass;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var data = new SunMass { };
        dstManager.AddComponentData(entity, data);
    }
}
