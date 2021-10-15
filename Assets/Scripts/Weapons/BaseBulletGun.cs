using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBulletGun : BaseItem
{
	[Header("Weapon")]
	public float reloadTime = 0.5f;
	public uint magazine = 3;
	private uint restMagazine = 3;

	private float lastShootingTime = 0.0f;
	public float shootingDelay = 0.25f;

	public Animation anim;
	public Transform muzzlePosition;

	[SerializeField]
	private GameObject ownedBullet;

	private void Awake()
	{
		
	}

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
			UIManager.Instance.weaponStatText.text = restMagazine + " / " + magazine;
		}
	}

	public bool Fire(BrawlerController controller)
	{
		if (restMagazine > 0)
		{
			if ((lastShootingTime + shootingDelay) <= Time.realtimeSinceStartup)
			{
				if(anim.GetClip("FireGun"))
				{
					anim.CrossFade("FireGun", 0.1f);
				}

				restMagazine -= 1;
				UIManager.Instance.weaponStatText.text = restMagazine + " / " + magazine;
				lastShootingTime = Time.realtimeSinceStartup;

				GameObject bulletObject = ObjectPoolingManager.SharedInstance.GetPooledObject(ownedBullet.tag);
				if (bulletObject != null)
				{
					bulletObject.GetComponent<Bullet>().SetBulletInfo(controller, muzzlePosition.position, transform.forward * 1.5f);
					bulletObject.SetActive(true);
					return true;
				}
			}
		}

		return false;
	}
}
