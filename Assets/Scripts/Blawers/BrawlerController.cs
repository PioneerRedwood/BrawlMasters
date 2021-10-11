using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class BrawlerController : MonoBehaviour
{
	[Header("Player Properties")]
	public float moveSpeed = 2.0f;
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

	public void OnMovement(InputAction.CallbackContext value)
	{
		Vector2 input = value.ReadValue<Vector2>();
		rawInputMovement = new Vector3(input.x, 0, input.y);
	}
	
	// deprecated 2020-10-11
	public void OnAttack(InputAction.CallbackContext value)
	{
		//Debug.Log($"OnAttack{value}");
		//if(gun != null && Keyboard.current.spaceKey.wasPressedThisFrame)
		//{
		//	gun.Fire(this);
		//}
	}

	// �Է¿� ���� ���� ������ - �ε巯�� �������� ����� �� ������ ��� ��Ȳ���� ����Ǵ� ���� �ƴ�
	void CalculateMovementInputSmoothing()
	{
		smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
	}

	void UpdatePlayerMovement()
	{
		// ���ϴ� �������� �ƴ�
		if (smoothInputMovement.sqrMagnitude > 0.01f)
		{
			Quaternion rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(smoothInputMovement),
				turnSpeed);

			body.MoveRotation(rotation);
		}

		transform.Translate(currMoveSpeed * smoothInputMovement.z * Time.deltaTime * Vector3.forward);
		transform.Translate(currMoveSpeed * smoothInputMovement.x * Time.deltaTime * Vector3.right);

	}
}
