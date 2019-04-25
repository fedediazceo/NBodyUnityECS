using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BodySpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public GameObject bodyPrefab;
    public int bodyCount = 10 ;
    public float spaceWidth;
    public float spaceHeight;
    public float spaceDepth;
    public float bodyMass;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var spawnerData = new BodySpawner {
            bodyEntity = conversionSystem.GetPrimaryEntity(bodyPrefab),
            bodyCount = this.bodyCount,
            spaceWidth = this.spaceWidth,
            spaceHeight = this.spaceHeight,
            spaceDepth = this.spaceDepth,
            bodyMass = this.bodyMass
        };
        dstManager.AddComponentData(entity, spawnerData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(bodyPrefab);
    }
}
