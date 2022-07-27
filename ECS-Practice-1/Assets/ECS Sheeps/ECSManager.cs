using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
	private EntityManager manager;
	public GameObject sheepPrefab;
	private const int numSheep = 15000;

	void Start()
	{
		manager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
		var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(sheepPrefab, settings);

		for (int i = 0; i < numSheep; i++)
		{
			var instance = manager.Instantiate(prefab);
			var position = transform.TransformPoint(new float3(UnityEngine.Random.Range(-50, 50), 0, UnityEngine.Random.Range(-50, 50)));
			manager.SetComponentData(instance, new Translation { Value = position });
			manager.SetComponentData(instance, new Rotation { Value = new quaternion(0, 0, 0, 0) });
		}
	}
}
