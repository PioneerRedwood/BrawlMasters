using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public float damage = 10.0f;
    private BrawlerController owner = null;
    public string type = "Basic";

    private Vector3 spawnedPosition;
    private Vector3 destination;
    public float distance = 50.0f;

    public void SetBulletInfo(BrawlerController owner, Vector3 spawnedPosition, Vector3 destination)
    {
        this.owner = owner;
        this.spawnedPosition = spawnedPosition;
        this.destination = destination;

		transform.SetPositionAndRotation(spawnedPosition, Quaternion.identity);
	}

    // Update�� �ִ� �ͺ��� Force�� �������� ���� �ִ°� �����
    private void LateUpdate()
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
        Debug.Log(owner.gameObject.name + " " + other.gameObject.name);

        if(other.CompareTag("Enemy"))
		{
            Debug.Log("Shot enemy!");
		}

        gameObject.SetActive(false);
    }

	private void OnTriggerExit(Collider other)
	{
        Debug.Log(owner.gameObject.name + " " + other.gameObject.name);
    }

	private void OnTriggerStay(Collider other)
	{
        Debug.Log(owner.gameObject.name + " " + other.gameObject.name);
    }
}
