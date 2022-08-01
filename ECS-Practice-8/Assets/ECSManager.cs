using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
	EntityManager manager;
	public GameObject shipPrefab;
	public GameObject bulletPrefab;
	const int numShips = 500;

	void Start()
	{
		manager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
		var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shipPrefab, settings);
		var bullet = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

		for (int i = 0; i < numShips; i++)
		{
			var instance = manager.Instantiate(prefab);
			float x = UnityEngine.Random.Range(-300, 300);
			float y = UnityEngine.Random.Range(-300, 300);
			float z = UnityEngine.Random.Range(-300, 300);
			var position = transform.TransformPoint(new float3(x, y, z));
			manager.SetComponentData(instance, new Translation { Value = position });

			var q = Quaternion.Euler(new Vector3(0, 45, 0));
			manager.SetComponentData(instance, new Rotation { Value = new quaternion(q.x, q.y, q.z, q.w) });

			int closesWP = 0;
			float distance = Mathf.Infinity;
			for (int j = 0; j < GameDataManager.instance.wps.Length; j++)
			{
				if (Vector3.Distance(GameDataManager.instance.wps[j], position) < distance)
				{
					closesWP = j;
					distance = Vector3.Distance(GameDataManager.instance.wps[j], position);
				}
			}

			manager.SetComponentData(instance, new ShipData {
				speed = UnityEngine.Random.Range(5, 20),
				rotationSpeed = UnityEngine.Random.Range(3, 5),
				currentWP = closesWP,
				bullet = bullet
			});
		}
	}
}
