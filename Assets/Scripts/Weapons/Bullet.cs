using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    public float speed = 10.0f;
    public float damage = 10.0f;
    public float distance = 50.0f;

    private BrawlerController owner = null;

    private Vector3 spawnedPosition;
    private Vector3 destination;

	public void SetBulletInfo(BrawlerController player, Vector3 spawnedPos)
    {
        owner = player;
        spawnedPosition = spawnedPos;
        destination = owner.gameObject.transform.forward;

        transform.SetPositionAndRotation(spawnedPosition, Quaternion.identity);
	}

    private void Update()
    {
        if (owner != null && isActiveAndEnabled)
        {
            transform.Translate(speed * Time.deltaTime * destination);
			//Debug.DrawLine(spawnedPosition, spawnedPosition + owner.gameObject.transform.forward * speed);          
		}

        if(Vector3.Distance(transform.position, spawnedPosition) >= (owner.isPowerUp > 0 ? distance * owner.powerIncrease : distance))
        {
            gameObject.SetActive(false);
        }
    }

	public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
		{
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            float offset = owner.isPowerUp > 0 ? damage * owner.powerIncrease : damage;

            enemy.OnDamage(offset);
		}

        gameObject.SetActive(false);
    }
    
}
