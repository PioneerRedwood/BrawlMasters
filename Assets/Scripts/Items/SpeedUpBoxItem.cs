using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBoxItem : BaseItem
{
    [Header("Speed Up Properites")]
    public float moveSpeedUp;
    public float attackSpeedUp;
    public float duration;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            // speed up the player movement for duration object move
            StartCoroutine("SpeedUpForDuration", other.GetComponent<BrawlerController>());
        }
	}

	IEnumerator SpeedUpForDuration(BrawlerController player)
	{
		isOwned = true;
        float prevMoveSpeed = player.moveSpeed;
        float prevShootingDelay = player.gun.shootingDelay;

		player.moveSpeed += moveSpeedUp;
        player.gun.shootingDelay -= attackSpeedUp;

		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<Animator>().enabled = false;

		yield return new WaitForSeconds(duration);

		// 플레이어 상태 변화
		player.moveSpeed = prevMoveSpeed;
		player.gun.shootingDelay = prevShootingDelay;

		// 지속시간 끝나면 오브젝트 비활성화
		isOwned = false;
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		gameObject.GetComponent<Animator>().enabled = true;
		gameObject.SetActive(false);
	}
}
