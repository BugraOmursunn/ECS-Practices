using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine.UI;

public class ECSInterface : MonoBehaviour
{
	private World world;
	private EntityManager entityManager;

	[SerializeField] private GameObject sheepPrefab;
	[SerializeField] private GameObject tankPrefab;
	[SerializeField] private GameObject palmTree;

	[Space(20)]
	[SerializeField] private Text sheepCountText;
	[SerializeField] private Text tankCountText;

	private void Start()
	{
		world = World.DefaultGameObjectInjectionWorld;
		entityManager = world.EntityManager;

		Debug.Log("All Entities: " + world.GetExistingSystem<MoveSystem>().EntityManager.GetAllEntities().Length);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
			SpawnSheep();
		else if (Input.GetKeyDown(KeyCode.T))
			SpawnTank();
		else if (Input.GetKeyDown(KeyCode.P))
			SpawnPalmTree();
	}
	private void SpawnSheep()
	{
		Vector3 position = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
		Instantiate(sheepPrefab, position, Quaternion.identity);
	}
	private void SpawnTank()
	{
		Vector3 position = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
		Instantiate(tankPrefab, position, Quaternion.identity);
	}
	private void SpawnPalmTree()
	{
		var settings = GameObjectConversionSettings.FromWorld(world, null);
		var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(palmTree, settings);

		var instance = entityManager.Instantiate(prefab);
		var position = transform.TransformPoint(new float3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10)));
		entityManager.SetComponentData(instance, new Translation { Value = position });
		entityManager.SetComponentData(instance, new Rotation { Value = new quaternion(0, 0, 0, 0) });
	}
	public void RefreshSheepCount()
	{
		EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<SheepData>());
		int sheepCount = entityQuery.CalculateEntityCount();
		//Debug.Log("Sheep Count: " + sheepCount);

		sheepCountText.text = sheepCount.ToString();
	}

	public void RefreshTankCount()
	{
		EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<TankData>());
		int tankCount = entityQuery.CalculateEntityCount();
		//Debug.Log("Sheep Count: " + tankCount);

		tankCountText.text = tankCount.ToString();
	}
}
