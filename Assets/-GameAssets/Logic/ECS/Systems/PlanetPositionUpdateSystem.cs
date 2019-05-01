using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlanetPositionUpdateSystem : JobComponentSystem
{

    public float simRate = 100.0f;

    [BurstCompile]
    struct PlanetMovementSystemJob : IJobForEachWithEntity<Translation, Velocity>
    {
        public float deltaTime;

        public void Execute(Entity entity, int index, ref Translation translation, ref Velocity velocity)
        {
            translation.Value += velocity.tempValue * deltaTime;
            velocity.Value = velocity.tempValue;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var job = new PlanetMovementSystemJob { deltaTime = Time.deltaTime * simRate };
        //a bit of cheating, I know there is only one sun for now
        return job.Schedule(this, inputDependencies);
    }

}