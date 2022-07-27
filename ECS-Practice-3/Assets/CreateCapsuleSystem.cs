using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Jobs;

public class CreateCapsuleSystem : JobComponentSystem
{
	protected override void OnCreate()
	{
		base.OnCreate();

		#region Mesh without rendering

		// var instance = EntityManager.CreateEntity(
		// 	ComponentType.ReadWrite<Translation>(),
		// 	ComponentType.ReadWrite<Rotation>(),
		// 	ComponentType.ReadOnly<RenderMesh>()
		// );
		//
		// EntityManager.SetComponentData(instance, new Translation { Value = new float3(0, 0, 0) });
		// EntityManager.SetComponentData(instance, new Rotation() { Value = new quaternion(0, 0, 0, 0) });

		#endregion

		#region Mesh with rendering

		// var instance = EntityManager.CreateEntity(
		// 	ComponentType.ReadOnly<LocalToWorld>(),
		// 	ComponentType.ReadOnly<RenderMesh>()
		// );
		//
		// EntityManager.SetComponentData(instance, new LocalToWorld {
		// 	Value = new float4x4(rotation: quaternion.identity, translation: new float3(0, 0, 0))
		// });

		#endregion

		#region Sheep

		for (int i = 0; i < 50; i++)
		{
			var instance = EntityManager.CreateEntity(
				ComponentType.ReadOnly<LocalToWorld>(),
				ComponentType.ReadWrite<Translation>(),
				ComponentType.ReadWrite<Rotation>(),
				ComponentType.ReadOnly<NonUniformScale>(),
				ComponentType.ReadOnly<RenderMesh>()
			);

			float3 pos = new float3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
			float scale = UnityEngine.Random.Range(1, 80);

			EntityManager.SetComponentData(instance, new LocalToWorld {
				Value = new float4x4(rotation: quaternion.identity, translation: pos)
			});

			EntityManager.SetComponentData(instance, new Translation { Value = pos });
			EntityManager.SetComponentData(instance, new Rotation() { Value = quaternion.identity });

			EntityManager.SetComponentData(instance, new NonUniformScale {
				Value = new float3(scale, scale, scale)
			});

			var rHolder = Resources.Load<GameObject>("ResourcesHolder").GetComponent<ResourcesHolder>();
			EntityManager.SetSharedComponentData(instance,
				new RenderMesh {
					mesh = rHolder.theMesh,
					material = rHolder.theMaterial
				});
		}

		#endregion
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		return inputDeps;
	}
}
