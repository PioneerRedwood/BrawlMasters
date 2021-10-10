using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BrawlerController : MonoBehaviour
{
	[Header("Player Properties")]
	public float moveSpeed = 2.0f;
	public float reloadTime = 0.5f;
	public uint magazine = 3;
	private uint restMagazine = 3;

	public Text textMagazine;
	public Transform muzzlePosition;

	private float lastShootingTime = 0.0f;
	public float shootingDelay = 0.25f;
	public GameObject bullet;

	public Animation anim;

	void Start()
	{
		lastShootingTime = Time.realtimeSinceStartup;
		InvokeRepeating("CheckReloading", 0.0f, reloadTime);
	}

	void CheckReloading()
	{
		if (restMagazine < magazine)
		{
			restMagazine += 1;
			textMagazine.text = restMagazine + " / " + magazine;
		}
	}

	private void FixedUpdate()
	{
		CalculateMovementInputSmoothing();
		UpdatePlayerMovement();
	}

	[Header("Player Input")]
	public float movementSmoothingSpeed = 1f;
	private Vector3 rawInputMovement;
	private Vector3 smoothInputMovement;

	public void OnMovement(InputAction.CallbackContext value)
	{
		Vector2 inputMovement = value.ReadValue<Vector2>();
		rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
	}

	public void OnAttack(InputAction.CallbackContext value)
	{
		if (restMagazine > 0)
		{
			if ((lastShootingTime + shootingDelay) <= Time.realtimeSinceStartup)
			{
				restMagazine -= 1;
				textMagazine.text = restMagazine+ " / " + magazine;
				lastShootingTime = Time.realtimeSinceStartup;

				GameObject bulletObject = ObjectPooler.SharedInstance.GetPooledObject();
				if(bulletObject != null)
				{
					if(anim.GetClip("Fire"))
					{
						anim.CrossFade("Fire", 0.1f);
					}

					bulletObject.GetComponent<Bullet>().SetBulletInfo(this, muzzlePosition.position, transform.forward * 1.5f);
					bulletObject.SetActive(true);
					//bulletObject.transform.SetParent(GameObject.Find("BulletPooler").transform);
				}
				
			}
		}
	}

	// 입력에 따른 보간 움직임 - 부드러운 움직임을 기대할 수 있으나 모든 상황에서 적용되는 것은 아님
	void CalculateMovementInputSmoothing()
	{
		smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
	}

	void UpdatePlayerMovement()
	{
		gameObject.transform.Translate(Vector3.forward * moveSpeed * smoothInputMovement.z * Time.deltaTime);
		gameObject.transform.Translate(Vector3.right * moveSpeed * smoothInputMovement.x * Time.deltaTime);
	}
}
