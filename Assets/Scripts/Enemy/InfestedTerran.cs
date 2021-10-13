using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfestedTerran : BaseEnemy
{

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            other.GetComponent<BrawlerController>().hp -= damage;
		}
	}
}
