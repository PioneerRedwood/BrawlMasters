using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Basic Properties")]
    protected float hp;
    protected float attackDamage;

    public void KillEnemy()
	{
        gameObject.SetActive(false);
	}

    public void OnDamage(float damage)
	{
        hp -= damage;
        if (hp <= 0)
		{
            KillEnemy();
		}
	}
}
