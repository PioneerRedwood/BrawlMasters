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
    private Rigidbody body;

	private void Awake()
	{
        body = GetComponent<Rigidbody>();
	}

	public void SetBulletInfo(BrawlerController owner, Vector3 spawnedPosition, Vector3 destination)
    {
        this.owner = owner;
        this.spawnedPosition = spawnedPosition;
        this.destination = destination;

		transform.SetPositionAndRotation(spawnedPosition, Quaternion.identity);
        
	}

    private void FixedUpdate()
    {
        if (owner != null)
        {
            //transform.Translate(speed * Time.deltaTime * destination);
            //body.MovePosition(speed * Time.deltaTime * destination);
            //body.AddForceAtPosition(speed * Time.deltaTime * destination, transform.position);
            // 총알을 어떻게 움직이는게 좋을지 구글링

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

            //Debug.Log($"BULLET INFO damage: {(owner.isPowerUp ? damage * owner.powerIncrease : damage)} " +
            //    $"distance:{(owner.isPowerUp ? distance * owner.powerIncrease : distance)}");
		}

        gameObject.SetActive(false);
    }
    
}
