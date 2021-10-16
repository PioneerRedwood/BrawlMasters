using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseItem
{
    [Header("Bullet Properties")]
    public float speed = 10.0f;
    public float damage = 10.0f;
    public float distance = 50.0f;

    private BrawlerController owner = null;

    private Vector3 spawnedPosition;
    private Vector3 destination;

    public void SetBulletInfo(BrawlerController owner, Vector3 spawnedPosition, Vector3 destination)
    {
        this.owner = owner;
        this.spawnedPosition = spawnedPosition;
        this.destination = destination;

		transform.SetPositionAndRotation(spawnedPosition, Quaternion.identity);
	}

    // Update�� �ִ� �ͺ��� Force�� �������� ���� �ִ°� �����
    private void FixedUpdate()
    {
        if (owner != null)
        {
            // Do your work
            transform.Translate(destination * speed * Time.deltaTime);
        }

        if(Vector3.Distance(transform.position, spawnedPosition) >= distance)
        {
            gameObject.SetActive(false);
        }
    }

	public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(owner.gameObject.name + " shoots " + other.gameObject.name);

        if(other.CompareTag("Enemy"))
		{
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            enemy.hp -= damage;
            if (enemy.hp <= 0.0f)
			{
                enemy.gameObject.SetActive(false);
			}
		}

        gameObject.SetActive(false);
    }
    
}
