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
        public int amount;
        public bool isExpand;
    }

    private void Awake()
	{
        SharedInstance = this;
	}

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

	// Start is called before the first frame update
	void Start()
    {
		pooledObjects = new List<GameObject>();
		for (int i = 0; i < amountToPool; ++i)
		{
			GameObject obj = (GameObject)Instantiate(objectToPool);
			obj.SetActive(false);
            obj.transform.SetParent(GameObject.Find("BulletsPooler").transform);
            pooledObjects.Add(obj);
		}
	}

    public GameObject GetPooledObject()
	{
        for(int i = 0; i < pooledObjects.Count; ++i)
		{
            if(!pooledObjects[i].activeInHierarchy)
			{
                return pooledObjects[i];
			}
		}

        return null;
	}
}
