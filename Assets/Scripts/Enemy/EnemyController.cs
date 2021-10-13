using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject target;

    [Header("Movement")]
    [Range(0, 0.1f)]
    public float maxSpeed = 0.05f;
    public float turnSpeed = 2.0f;
    public Rigidbody body;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, maxSpeed);

        Vector3 targetDirection = target.transform.position - transform.position;
        float singleStep = turnSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

    }

	private void OnTriggerEnter(Collider other)
	{
        
    }
}
