
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

public class PlanetMovementSystem : JobComponentSystem
{
    const float Softening2 = 3e4f * 3e4f;
    public float simRate = 100.0f;
    EntityCommandBufferSystem m_EntityCommandBufferSystem;
    NativeArray<SunMass> sunsMasses;
    NativeArray<Translation> sunsPositions;
    //[BurstCompile]
    protected override void OnCreateManager() {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();
    }
    [BurstCompile]
    struct PlanetMovementSystemJob : IJobForEachWithEntity<Translation, Velocity,Mass>
    {
        public float deltaTime;
        public SunMass sunMass;
        public Translation sunPosition;
        public float G;
        public void Execute(Entity entity, int index, ref Translation translation, ref Velocity velocity, [ReadOnly]ref Mass mass) {
            float3 acceleration = float3(0, 0, 0);
            float3 direction = sunPosition.Value - translation.Value;
            float radius = length(direction);
            float force = G * mass.Value * sunMass.Value / (radius * radius + PlanetMovementSystem.Softening2);
            acceleration += force * normalize(direction);

            velocity.Value += acceleration * deltaTime;
            translation.Value += velocity.Value * deltaTime;
            //Debug.LogFormat("radius {0} for entity {1}", radius, index);
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {

        var job = new PlanetMovementSystemJob { G = 6.67300e-11f, deltaTime = Time.deltaTime*simRate };
        //a bit of cheating, I know there is only one sun for now
        if(sunsMasses.Length == 0) {
            EntityQuery m_Group = GetEntityQuery(typeof(SunMass), typeof(Translation));
            sunsMasses = m_Group.ToComponentDataArray<SunMass>(Allocator.Persistent);
            sunsPositions = m_Group.ToComponentDataArray<Translation>(Allocator.Persistent);
        }
        job.sunMass = sunsMasses[0];
        job.sunPosition = sunsPositions[0];
        sunsMasses.Dispose();
        sunsPositions.Dispose();
        return job.Schedule(this, inputDependencies);
    }
}