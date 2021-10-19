using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBoxItem : BaseItem
{
    [Header("Power Up Properites")]
    public float damageUp;
    public float rangeUp;
    public float duration;

	private void FixedUpdate()
	{
        if (owner != null)
        {
            transform.Translate(owner.transform.position);
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player"))
        {
            StartCoroutine(nameof(PowerUpForDuration), other.GetComponent<BrawlerController>());
        }
    }

    IEnumerator PowerUpForDuration(BrawlerController player)
	{
        owner = player.gameObject;
        float prevAttackDamage = player.GetOwnedBullet().damage;
        float prevAttackRange = player.GetOwnedBullet().distance;

        player.GetOwnedBullet().damage += damageUp;
        player.GetOwnedBullet().distance += rangeUp;

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<ParticleSystem>().Stop();

        yield return new WaitForSeconds(duration);

        owner = null;
        player.GetOwnedBullet().damage = prevAttackDamage;
        player.GetOwnedBullet().distance = prevAttackRange;

        // 지속시간 끝나면 오브젝트 비활성화
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }
}
