using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Jobs;
using Unity.Jobs;
using UnityEngine.Jobs;

public class SpawnParallel : MonoBehaviour
{
	struct MoveJob: IJobParallelForTransform
	{
		public void Execute(int index, TransformAccess transform)
		{
			transform.position += 0.1f * (transform.rotation * new Vector3(0, 0, 1));
			if (transform.position.z > 50)
			{
				transform.position = new Vector3(transform.position.x, 0, -50);
			}
		}
	}
	private MoveJob moveJob;
	private JobHandle moveHandle;
	private TransformAccessArray transforms;
	
	public GameObject sheepPrefab;
	const int numSheep = 15000;
	public List<Transform> sheep;
	// Start is called before the first frame update
	void Start()
	{
		for (int i = 0; i < numSheep; i++)
		{
			Vector3 pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
			GameObject newSheep = Instantiate(sheepPrefab, pos, Quaternion.identity);
			sheep.Add(newSheep.transform);
		}
		transforms = new TransformAccessArray(sheep.ToArray());
	}
	private void Update()
	{
		moveJob = new MoveJob { };
		moveHandle = moveJob.Schedule(transforms);
	}
	private void LateUpdate()
	{
		moveHandle.Complete();
	}
	private void OnDestroy()
	{
		transforms.Dispose();
	}
}
