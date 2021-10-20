using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class BrawlerController : MonoBehaviour
{
	[Header("Player Properties")]
	[SerializeField]
	private float hp = 200;
	
	[Range(0, 5.0f)]
	[SerializeField]
	private float moveSpeed = 2.0f;
	
	[Range(0, 1.0f)]
	[SerializeField]
	private float turnSpeed = 2.0f;

	[SerializeField]
	private float currMoveSpeed = 2.0f;
	private Rigidbody body;

	[Header("Weapon Properties")]
	[SerializeField]
	private BulletGun gun;

	public uint isSpeedUp;
	public float speedIncrease;
	public uint isPowerUp;
	public float powerIncrease;

	void Start()
	{
		currMoveSpeed = moveSpeed;
		body = GetComponentInChildren<Rigidbody>();
	}

	private void FixedUpdate()
	{
		// Player doing
		FireWeapon();

		// Player movement
		CalculateMovementInputSmoothing();
		UpdatePlayerRotation();
		UpdatePlayerMovement();
	}

	private void FireWeapon()
	{
		if (gun != null && Keyboard.current.spaceKey.isPressed)
		{
			gun.Fire(this);
		}
	}

	[Header("Player Input")]
	[SerializeField]
	private float movementSmoothingSpeed = 1f;
	private Vector3 rawInputMovement;
	private Vector3 smoothInputMovement;
	[SerializeField]
	private bool isLerpInput;

	public void OnMovement(InputAction.CallbackContext value)
	{
		Vector2 input = value.ReadValue<Vector2>();
		rawInputMovement = new Vector3(input.x, 0, input.y);
	}
	
	void CalculateMovementInputSmoothing()
	{
		if(isLerpInput)
		{
			smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
		}
		else
		{
			smoothInputMovement = rawInputMovement;
		}
	}

	void UpdatePlayerRotation()
	{
		if (smoothInputMovement.sqrMagnitude > 0.01f)
		{
			Quaternion rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(smoothInputMovement),
				turnSpeed);

			body.MoveRotation(rotation);
		}
	}

	void UpdatePlayerMovement()
	{
		float offset = (isSpeedUp > 0 ? currMoveSpeed * speedIncrease : currMoveSpeed);
		Vector3 movement = offset * Time.deltaTime * smoothInputMovement;

		if (movement.magnitude > 0.01f)
		{
			body.MovePosition(transform.position + movement);
		}
	}

	public void OnDamage(float damage)
	{
		hp -= damage;
		if(hp <= 0)
		{
			Debug.Log("-----YOU ARE DEAD-----");
		}
	}
}