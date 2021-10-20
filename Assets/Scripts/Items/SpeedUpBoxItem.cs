using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBoxItem : BaseItem
{
    [Header("Speed Up Properites")]
    [SerializeField]
	[Range(1.0f, 2.0f)]
    private float increase;
	[SerializeField]
	private float duration;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			StartCoroutine(nameof(SpeedUpForDuration), other.GetComponent<BrawlerController>());
		}
	}

	IEnumerator SpeedUpForDuration(BrawlerController player)
	{
		owner = player.gameObject;
		player.isSpeedUp++;
		player.speedIncrease += increase;

		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Animator>().enabled = false;
		GetComponent<ParticleSystem>().Stop();

		yield return new WaitForSeconds(duration);

		owner = null;
		player.isSpeedUp--;
		player.speedIncrease -= increase;

		GetComponent<MeshRenderer>().enabled = true;
		GetComponent<Animator>().enabled = true;
		GetComponent<ParticleSystem>().Play();
		gameObject.SetActive(false);
	}
}
