using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine.UIElements;

public class MoveSystem : JobComponentSystem
{
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var JobHandle = Entities.WithName("MoveSystem").ForEach((ref Translation position, ref Rotation rotation) => {
			
			position.Value += 0.1f * math.forward(rotation.Value); //this declare where it is facing with
			
			if (position.Value.z > 50)
				position.Value.z = -50;
			
			// position.Value.y += 0.1f ;
			// if (position.Value.y > 10)
			// 	position.Value.y = 0;
		}).Schedule(inputDeps);
		return JobHandle;
	}
}
