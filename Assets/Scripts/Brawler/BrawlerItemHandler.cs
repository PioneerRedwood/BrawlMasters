using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerItemHandler : MonoBehaviour
{
    BrawlerController controller;
    List<BaseItem> items;

    public void GetItem(BaseItem item)
	{
        // SpeedUp
        
        // DamageUp

        // Healing

        // Barrier

        // Dibuff
	}


	private void Awake()
	{
        controller = GetComponent<BrawlerController>();
	}



	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
