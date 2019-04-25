using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class SunSpawnerSystem : JobComponentSystem {

    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreateManager() {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    struct SpawnJob : IJobForEachWithEntity<SunSpawner, LocalToWorld> {
        public EntityCommandBuffer commandBuffer;

        public void Execute(Entity entity, int index, [ReadOnly]ref SunSpawner sunSpawner, [ReadOnly]ref LocalToWorld location) {

            var instance = commandBuffer.Instantiate(sunSpawner.prefabSun);

            var position = math.transform(location.Value, sunSpawner.sunPosition);

            commandBuffer.SetComponent(instance, new Translation { Value = position });
            commandBuffer.SetComponent(instance, new SunMass { Value = sunSpawner.sunMass });
            
            commandBuffer.DestroyEntity(entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        var job = new SpawnJob {
            commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
        }.ScheduleSingle(this, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
