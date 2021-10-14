using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class BrawlerController : MonoBehaviour
{
	[Header("Player Properties")]
	public float hp = 200;
	public float moveSpeed = 2.0f;
	[Range(0, 1.0f)]
	public float turnSpeed = 2.0f;

	private float currMoveSpeed = 2.0f;
	private float speedUpVolume = 1.25f;
	private Rigidbody body;

	[Header("Weapon")]
	public BaseBulletGun gun;

	void Start()
	{
		currMoveSpeed = moveSpeed;
		body = GetComponentInChildren<Rigidbody>();
	}

	private void FixedUpdate()
	{
		CalculateMovementInputSmoothing();
		UpdatePlayerMovement();

		if (gun != null && Keyboard.current.spaceKey.isPressed)
		{
			gun.Fire(this);
		}

		if(Keyboard.current.leftShiftKey.isPressed)
		{
			currMoveSpeed = moveSpeed * speedUpVolume;
		}
		else
		{
			currMoveSpeed = moveSpeed;
		}
	}

	[Header("Player Input")]
	public float movementSmoothingSpeed = 1f;
	private Vector3 rawInputMovement;
	private Vector3 smoothInputMovement;
	public bool isLerpInput;

	public void OnMovement(InputAction.CallbackContext value)
	{
		Vector2 input = value.ReadValue<Vector2>();
		rawInputMovement = new Vector3(input.x, 0, input.y);
	}
	
	// �Է¿� ���� ���� ������ - �ε巯�� �������� ����� �� ������ ��� ��Ȳ���� ����Ǵ� ���� �ƴ�
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

	void UpdatePlayerMovement()
	{
		if (smoothInputMovement.sqrMagnitude > 0.01f)
		{
			Quaternion rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(smoothInputMovement),
				turnSpeed);

			body.MoveRotation(rotation);
		}

		Vector3 movement = currMoveSpeed * Time.deltaTime * smoothInputMovement + transform.position;
		body.MovePosition(movement);

	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
	}
}
