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
        // 효율적인지 모르겠음
        InvokeRepeating("ForceToMove", 0.0f, 0.01f);
    }

    void ForceToMove()
    {
        Vector3 targetDirection = target.transform.position - transform.position;

        body.AddForce(targetDirection);
    }

    void FixedUpdate()
    {
        // Move
        //body.MovePosition(target.transform.position);
        
        // Rotate
        Vector3 targetDirection = target.transform.position - transform.position;

        //body.AddForce(targetDirection, ForceMode.Force);

        if (targetDirection.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(targetDirection),
                turnSpeed);

            body.MoveRotation(rotation);
        }

        
        //float singleStep = turnSpeed * Time.deltaTime;

        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDirection);

    }

	private void OnTriggerEnter(Collider other)
	{
        
    }
}
