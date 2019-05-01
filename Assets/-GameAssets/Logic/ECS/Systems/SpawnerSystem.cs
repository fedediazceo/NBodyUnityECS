using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class BodySpawnerSystem : JobComponentSystem {

    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreateManager() {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    struct SpawnJob : IJobForEachWithEntity<BodySpawner, LocalToWorld> {
        public EntityCommandBuffer commandBuffer;
        public Unity.Mathematics.Random random;
        public void Execute(Entity entity, int index, [ReadOnly]ref BodySpawner bodySpawner, [ReadOnly]ref LocalToWorld location) {

            for (int i = 0; i < bodySpawner.bodyCount; i++) {
                var instance = commandBuffer.Instantiate(bodySpawner.bodyEntity);

                var position = math.transform(location.Value, new float3(
                            random.NextFloat(-bodySpawner.spaceHeight, bodySpawner.spaceHeight),
                            random.NextFloat(-bodySpawner.spaceWidth, bodySpawner.spaceWidth),
                            random.NextFloat(-bodySpawner.spaceWidth, bodySpawner.spaceWidth)));

                var velocity = new float3(
                    random.NextFloat(-1, 1),
                    random.NextFloat(-1, 1),
                    random.NextFloat(-1, 1));

                commandBuffer.SetComponent(instance, new Translation { Value = position });
                commandBuffer.SetComponent(instance, new Velocity { Value = velocity, tempValue = velocity });
                commandBuffer.SetComponent(instance, new Mass { Value = bodySpawner.bodyMass });
            }
            commandBuffer.DestroyEntity(entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        var job = new SpawnJob {
            commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
            random = new Unity.Mathematics.Random()
        };

        job.random.InitState((uint)System.DateTime.Now.ToBinary());
        var jobHandle = job.ScheduleSingle(this, inputDeps);
        m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
