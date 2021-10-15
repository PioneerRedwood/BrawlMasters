using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBoxItem : BaseItem
{
    [Header("Speed Up Properites")]
    public float moveSpeedUp;
    public float attackSpeedUp;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            // speed up the player movement for duration
            // object move 
            StartCoroutine("SpeedUpForDuration", other.GetComponent<BrawlerController>());

        }
	}

 //   IEnumerator SpeedUpForDuration(BrawlerController player)
	//{
 //       isOwned = true;

		//player.currMoveSpeed += moveSpeedUp;
		//player.gun.shootingDelay

  //  	yield return new WaitForSeconds(duration);

  //      isOwned = false;
  //      gameObject.SetActive(false);


	}
}
