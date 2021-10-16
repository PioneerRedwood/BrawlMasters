using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target;

    [Header("Movement")]
    [Range(0, 3.0f)]
    public float moveSpeed = 1.2f;
    public float turnSpeed = 2.0f;
    public Rigidbody body;

    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {

		// Rotate
		Vector3 targetPos = target.position - transform.position;
             
        if (targetPos.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(targetPos),
                turnSpeed);

            body.MoveRotation(rotation);
        }
        
        // Move
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            target.position *= -1.0f;
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        
    }
}
