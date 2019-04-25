using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[Serializable]
public class SunSpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public GameObject sunPrefab;
    public Vector3 sunPosition = new Vector3(0, 0, 0);
    public float sunMass = 100000;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var sunSpawnerData = new SunSpawner {
            prefabSun = conversionSystem.GetPrimaryEntity(sunPrefab),
            sunPosition = this.sunPosition,
            sunMass = this.sunMass
        };

        dstManager.AddComponentData(entity, sunSpawnerData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(sunPrefab);

    }
}
