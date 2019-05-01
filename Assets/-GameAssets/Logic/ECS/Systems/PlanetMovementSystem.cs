
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

[UpdateBefore(typeof(PlanetPositionUpdateSystem))]
public class PlanetMovementSystem : JobComponentSystem
{
    const float Softening2 = 3e4f * 3e4f;
    public float simRate = 100.0f;

    EntityCommandBufferSystem m_EntityCommandBufferSystem;

    NativeArray<SunMass> sunsMasses;
    NativeArray<Translation> sunsPositions;


    protected override void OnCreateManager() {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();

    }

    //[BurstCompile]
    struct PlanetMovementSystemJob : IJobForEachWithEntity<Translation,Velocity,Mass>
    {
        public float deltaTime;

        [DeallocateOnJobCompletion][ReadOnly] public NativeArray<Mass> bodyMasses;
        [DeallocateOnJobCompletion][ReadOnly] public NativeArray<Translation> bodyTranslations;

        public float G;
        public void Execute(Entity entity, int index, [ReadOnly]ref Translation translation, ref Velocity velocity, [ReadOnly]ref Mass mass) {
            for (int i = 0; i < bodyMasses.Length;i++){
                if (i == index) continue;
                float3 acceleration = float3(0, 0, 0);

                float3 direction = bodyTranslations[i].Value - translation.Value;
                float radius = length(direction);
                float force = G * mass.Value * bodyMasses[i].Value / (radius * radius + PlanetMovementSystem.Softening2);
                acceleration += force * normalize(direction);

                velocity.tempValue += acceleration * deltaTime;
            }
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {

        var job = new PlanetMovementSystemJob { G = 6.67300e-11f, deltaTime = Time.deltaTime*simRate };
        //a bit of cheating, I know there is only one sun for now
        job.bodyMasses =  GetEntityQuery(typeof(Mass)).ToComponentDataArray<Mass>(Allocator.TempJob);
        job.bodyTranslations = GetEntityQuery(typeof(Translation)).ToComponentDataArray<Translation>(Allocator.TempJob);

        return job.Schedule(this, inputDependencies);
    }
}