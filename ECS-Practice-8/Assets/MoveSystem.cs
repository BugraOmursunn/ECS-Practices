﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

public class MoveSystem : JobComponentSystem
{
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		float deltaTime = Time.DeltaTime;
		NativeArray<float3> waypointPositions = new NativeArray<float3>(GameDataManager.instance.wps, Allocator.TempJob);
		var jobHandle = Entities
			.WithName("MoveSystem")
			.ForEach((ref Translation position, ref Rotation rotation, ref ShipData shipData) => {
				float distance = math.distance(position.Value, waypointPositions[shipData.currentWP]);

				if (distance < 60)
					shipData.approach = false;
				else if (distance > 200)
					shipData.approach = true;


				float3 heading;
				if (shipData.approach == true)
					heading = waypointPositions[shipData.currentWP] - position.Value;
				else
					heading = (waypointPositions[shipData.currentWP] - position.Value) * -1;

				quaternion targetDirection = quaternion.LookRotation(heading, math.up());
				rotation.Value = math.slerp(rotation.Value, targetDirection, deltaTime * shipData.rotationSpeed);
				position.Value += deltaTime * shipData.speed * math.forward(rotation.Value);
			})
			.Schedule(inputDeps);

		waypointPositions.Dispose(jobHandle);

		return jobHandle;

		// if (math.distance(position.Value, waypointPositions[shipData.currentWP]) < 10)
		// {
		//     shipData.currentWP++;
		//     if (shipData.currentWP >= waypointPositions.Length)
		//         shipData.currentWP = 0;
		// }
	}
}
