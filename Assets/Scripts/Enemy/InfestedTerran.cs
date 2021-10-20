using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfestedTerran : BaseEnemy
{
	// 2021-10-20 ������ ��� �ؾ�?

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            other.GetComponent<BrawlerController>().OnDamage(attackDamage);
		}
	}
}
