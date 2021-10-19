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
	
	[Range(0, 5.0f)]
	public float moveSpeed = 2.0f;
	
	[Range(0, 1.0f)]
	public float turnSpeed = 2.0f;
	
	public float currMoveSpeed = 2.0f;
	private float speedUpVolume = 1.25f;
	private Rigidbody body;

	[Header("Weapon Properties")]
	[SerializeField]
	private BaseBulletGun gun;

	public bool isSpeedUp;
	public float speedUpAmount;
	public bool isPowerUp;
	public float powerUpAmount;

	public BaseBulletGun GetOwnedGun()
	{
		return gun;
	}

	public Bullet GetOwnedBullet()
	{
		return gun.GetBullet();
	}

	void Start()
	{
		currMoveSpeed = moveSpeed;
		body = GetComponentInChildren<Rigidbody>();
	}

	private void FixedUpdate()
	{
		// Player doing
		FireWeapon();
		//OnSpeedUp();

		// Player movement
		CalculateMovementInputSmoothing();
		UpdatePlayerRotation();
		UpdatePlayerMovement();

	}

	void FireWeapon()
	{
		if (gun != null && Keyboard.current.spaceKey.isPressed)
		{
			gun.Fire(this);
		}
	}

	// Shift 키 누르면 이동속도 소폭 상승
	void OnSpeedUp()
	{
		if (Keyboard.current.leftShiftKey.isPressed)
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
	
	// 입력에 따른 보간 움직임 - 부드러운 움직임을 기대할 수 있으나 모든 상황에서 적용되는 것은 아님
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
		float offset = (isSpeedUp ? currMoveSpeed * speedUpAmount : currMoveSpeed);
		Vector3 movement = offset * Time.deltaTime * smoothInputMovement;
		//Debug.Log($"Force {movement.magnitude}");

		if (movement.magnitude > 0.01f)
		{
			body.MovePosition(transform.position + movement);
		}
	}
}