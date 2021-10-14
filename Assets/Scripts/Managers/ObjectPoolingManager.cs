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

	public List<ObjectPoolItem> itemsToPool;
	public List<GameObject> pooledObjects;
	public Vector2 randomPositionRange;

	private void Awake()
	{
		SharedInstance = this;

		pooledObjects = new List<GameObject>();

		foreach (ObjectPoolItem item in itemsToPool)
		{
			for (int i = 0; i < item.amount; ++i)
			{
				GameObject obj = Instantiate(item.poolItem);
				obj.name = item.poolItem.name;
				if(item.activeStarted)
				{
					obj.SetActive(true);
				}
				else
				{
					obj.SetActive(false);
				}

				obj.transform.SetParent(item.parentPooler.transform);
				if(item.randomlyPositioned)
				{
					obj.transform.TransformPoint(new Vector3(Random.Range(randomPositionRange.x, randomPositionRange.y), 0, Random.Range(randomPositionRange.x, randomPositionRange.y)));
				}
				
				pooledObjects.Add(obj);

			}
		}
	}

	public void PoolNamedObject(string name)
	{
		foreach (ObjectPoolItem item in itemsToPool)
		{
			if(item.poolItem.name.Equals(name))
			{
				bool pooled = false;
				for(int i = 0; i < item.amount; ++i)
				{
					if(pooled)
					{
						return;
					}

					Debug.Log($"{pooledObjects[i].name.Equals(name)} && {!pooledObjects[i].activeSelf}");
					if (pooledObjects[i].name.Equals(name) && !pooledObjects[i].activeSelf)
					{
						pooledObjects[i].SetActive(true);
						pooled = true;
					}
				}
			}
			else
			{
				continue;
			}
		}
	}

	void Start()
	{
		
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
}
