using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBoxItem : BaseItem
{
    [Header("Power Up Properites")]
    [Range(1.0f, 2.0f)]
    [SerializeField]
    private float increase;
    [SerializeField]
    private float duration;

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player"))
        {
            StartCoroutine(nameof(PowerUpForDuration), other.GetComponent<BrawlerController>());
        }
    }

    IEnumerator PowerUpForDuration(BrawlerController player)
	{
        if (owner != null)
		{
            yield return new WaitForSeconds(0);
        }

        owner = player.gameObject;
        player.isPowerUp++;
        player.powerIncrease += increase;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<ParticleSystem>().Stop();

        yield return new WaitForSeconds(duration);

        owner = null;
        player.isPowerUp--;
        player.powerIncrease -= increase;

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Animator>().enabled = true;
        GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }
}
