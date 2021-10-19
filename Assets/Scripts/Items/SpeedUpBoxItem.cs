using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBoxItem : BaseItem
{
    [Header("Speed Up Properites")]
    [SerializeField]
	[Range(1.0f, 2.0f)]
    private float speedUpAmount;
	[SerializeField]
	private float duration;

	private void FixedUpdate()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			StartCoroutine(nameof(SpeedUpForDuration), other.GetComponent<BrawlerController>());
			//other.GetComponent<BrawlerController>().SetStatus(this, SpeedUpForDuration);
		}
	}

	IEnumerator SpeedUpForDuration(BrawlerController player)
	{
		owner = player.gameObject;

		player.isSpeedUp = true;
		player.speedUpAmount += speedUpAmount;


		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<Animator>().enabled = false;
		gameObject.GetComponent<ParticleSystem>().Stop();

		yield return new WaitForSeconds(duration);

		owner = null;
		player.isSpeedUp = false;
		player.speedUpAmount -= speedUpAmount;

		// 지속시간 끝나면 오브젝트 비활성화
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		gameObject.GetComponent<Animator>().enabled = true;
		gameObject.GetComponent<ParticleSystem>().Play();
		gameObject.SetActive(false);

	}
}
