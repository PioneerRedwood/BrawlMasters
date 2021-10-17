using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
	public static ObjectPoolingManager SharedInstance;

	[System.Serializable]
	public class ObjectPoolItem
	{
		public GameObject poolItem;
		public GameObject parentPooler;
		public int amount;
		public bool shouldExpand;
		public bool randomlyPositioned;
		public bool activeStarted;

		enum EnumPooledType
		{
			Normal,
			WavePooling
		}

		[SerializeField]
		EnumPooledType polledType;
	}

	public List<ObjectPoolItem> itemsToPool;
	public List<GameObject> pooledObjects;

	// Depending on the size of map -11, 60, -30, 53
	private Vector2 randomXAxis = new Vector2(-11, 60);
	private Vector2 randomZAxis = new Vector2(-30, 53);

	private void Awake()
	{
		Random.InitState(12345);
		SharedInstance = this;

		pooledObjects = new List<GameObject>();

		foreach (ObjectPoolItem item in itemsToPool)
		{
			for (int i = 0; i < item.amount; ++i)
			{
				GameObject obj = Instantiate(item.poolItem);
				obj.name = item.poolItem.name;
				obj.SetActive(item.activeStarted);

				obj.transform.SetParent(item.parentPooler.transform);
				if(item.randomlyPositioned)
				{
					obj.transform.Translate(
						new Vector3(Random.Range(randomXAxis.x, randomXAxis.y),
									0,
									Random.Range(randomZAxis.x, randomZAxis.y)));
				}
				
				pooledObjects.Add(obj);
			}
		}
	}

	public bool PoolNamedObject(string name)
	{
		int idx = 0;
		foreach (ObjectPoolItem item in itemsToPool)
		{
			if(item.poolItem.name.Equals(name))
			{
				for(int i = idx; i < idx + item.amount; ++i)
				{
					if (!pooledObjects[i].activeSelf)
					{
						pooledObjects[i].SetActive(true);
						return true;
					}
				}
			}
			else
			{
				idx += item.amount;
				continue;
			}
		}

		return false;
	}

	public bool PoolNamedObject(string name, bool randomly)
	{
		int idx = 0;
		foreach (ObjectPoolItem item in itemsToPool)
		{
			if (item.poolItem.name.Equals(name))
			{
				for (int i = idx; i < idx + item.amount; ++i)
				{
					if (!pooledObjects[i].activeSelf)
					{
						pooledObjects[i].SetActive(true);
						if (randomly)
						{
							pooledObjects[i].transform.Translate(
								new Vector3(Random.Range(randomXAxis.x, randomXAxis.y), 
											0, 
											Random.Range(randomZAxis.x, randomZAxis.y)));
						}
						return true;
					}
				}
			}
			else
			{
				idx += item.amount;
				continue;
			}
		}

		return false;
	}

	public GameObject GetPooledObject(string tag)
	{
		for (int i = 0; i < pooledObjects.Count; ++i)
		{
			if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(tag))
			{
				return pooledObjects[i];
			}
		}

		foreach (ObjectPoolItem item in itemsToPool)
		{
			if(item.poolItem.CompareTag(tag) && item.shouldExpand)
			{
				GameObject obj = Instantiate(item.poolItem);
				obj.SetActive(false);
				obj.transform.SetParent(item.parentPooler.transform);
				pooledObjects.Add(obj);
				return obj;
			}
		}

		return null;
	}

	[System.Serializable]
	public struct Wave
	{
		public bool started;
		public GameObject enemyObject;
		public int amounts;
		public float createInterval;
	}

	public List<Wave> waves;
	[SerializeField]
	private float waveInterval;

	private int currWaveIdx;

	void Start()
	{
		currWaveIdx = 0;
		StartCoroutine(nameof(GenerateEnemy), currWaveIdx);
	}

	IEnumerator GenerateEnemy(int idx)
	{
		for (int i = 0; i < waves[idx].amounts; ++i)
		{
			PoolNamedObject(waves[idx].enemyObject.name, true);
			yield return new WaitForSeconds(waves[idx].createInterval);
		}

		currWaveIdx++;
		if (currWaveIdx < waves.Count)
		{
			StartCoroutine(nameof(GenerateEnemy), currWaveIdx);
		}
	}
}
