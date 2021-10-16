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

	public float currMoveSpeed = 2.0f;
	private float speedUpVolume = 1.25f;
	private Rigidbody body;

	[Header("Weapon")]
	public BaseBulletGun gun;

	void Start()
	{
		currMoveSpeed = moveSpeed;
		body = GetComponentInChildren<Rigidbody>();
		//axisLimits = new float[] { -21, 72, -35, 57 };
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

	//private float[] axisLimits;

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

		// ��������, +X-X-Z+Z
		// ���� �÷��̾ �ϼ��� �Ѱ輱�� ������ ���̻� ī�޶�� +Z, -X �������δ� �� ������
		//float moveX = movement.x, moveZ = movement.z;
		//if ((movement.x <= axisLimits[0]) || (movement.x >= axisLimits[1]))
		//{
		//	moveX = transform.position.x;
		//}
		//if ((movement.z <= axisLimits[2]) || (movement.z >= axisLimits[3]))
		//{
		//	moveZ = transform.position.z;
		//}

		//body.MovePosition(new Vector3(moveX, transform.position.y, moveZ));
	}
}
