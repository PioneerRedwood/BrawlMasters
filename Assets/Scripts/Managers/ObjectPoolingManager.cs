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
	}
	[Header("Object Pooling")]
	[SerializeField]
	private Vector2 randomXAxis = new Vector2(10, 40);
	[SerializeField]
	private Vector2 randomZAxis = new Vector2(10, 40);
	[SerializeField]
	private Vector2 randomPosOffset;

	[SerializeField]
	private List<ObjectPoolItem> itemsToPool;
	[SerializeField]
	private List<GameObject> pooledObjects;

	private void Awake()
	{
		SharedInstance = this;

		randomXAxis.x -= randomPosOffset.x;
		randomXAxis.y -= randomPosOffset.x;
		randomZAxis.x -= randomPosOffset.y;
		randomZAxis.y -= randomPosOffset.y;

		pooledObjects = new List<GameObject>();

		foreach (ObjectPoolItem item in itemsToPool)
		{
			for (int i = 0; i < item.amount; ++i)
			{
				GameObject obj = Instantiate(item.poolItem);
				obj.name = item.poolItem.name;
				obj.SetActive(item.activeStarted);

				obj.transform.SetParent(item.parentPooler.transform);
				if (item.randomlyPositioned)
				{
					obj.transform.Translate(
						new Vector3(Random.Range(randomXAxis.x, randomXAxis.y),
									0,
									Random.Range(randomZAxis.x, randomZAxis.y)));
				}

				pooledObjects.Add(obj);
			}

			if (item.poolItem.name.Contains("BoxItem"))
			{
				StartCoroutine(nameof(SpawnBoxItem), item.poolItem.name);
			}
		}
	}

	IEnumerator SpawnBoxItem(string itemName)
	{
		if(PoolNamedObject(itemName, true))
		{
			yield return new WaitForSeconds(5.0f);
			StartCoroutine(nameof(SpawnBoxItem), itemName);
		}
		else
		{
			yield return new WaitForSeconds(20.0f);
			StartCoroutine(nameof(SpawnBoxItem), itemName);
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

	#region Wave managing
	
	[System.Serializable]
	public struct Wave
	{
		public bool started;
		public GameObject enemyObject;
		public int amounts;
		public float createInterval;
	}
	[Header("Wave")]
	[SerializeField]
	private List<Wave> waves;

	[SerializeField]
	private float waveInterval;

	[Header("Wave")]
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
	#endregion
}
