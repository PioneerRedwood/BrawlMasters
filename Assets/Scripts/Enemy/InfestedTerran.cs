using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfestedTerran : BaseEnemy
{
	// 2021-10-20 공격을 어떻게 해야?

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            other.GetComponent<BrawlerController>().OnDamage(attackDamage);
		}
	}
}
