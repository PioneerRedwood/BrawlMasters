using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler SharedInstance;

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

	private void Awake()
	{
		SharedInstance = this;

		pooledObjects = new List<GameObject>();

		foreach (ObjectPoolItem item in itemsToPool)
		{
			for (int i = 0; i < item.amount; ++i)
			{
				GameObject obj = Instantiate(item.poolItem);
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
					obj.transform.Translate(new Vector3(Random.Range(0.0f, 1000.0f), 1, Random.Range(0.0f, 1000.0f)));
				}
				
				pooledObjects.Add(obj);

			}
		}
	}

	public List<ObjectPoolItem> itemsToPool;
	public List<GameObject> pooledObjects;

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
