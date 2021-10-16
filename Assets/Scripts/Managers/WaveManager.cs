using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
	{
        public bool started;
        public GameObject enemyObject;
        public int amounts;
        public float createInterval;
	}

    public List<Wave> waves;

    [SerializeField]
    private ObjectPoolingManager pooler;
    [SerializeField]
    private float waveInterval;

    void Start()
    {
        InvokeRepeating("WaveUpdate", 0.0f, waveInterval);
    }

    void WaveUpdate()
	{
        foreach(Wave wave in waves)
		{
            if(!wave.started)
			{
                StartCoroutine("generateEnemy", wave);
                UIManager.Instance.waveText.text = "Wave " + (waves.IndexOf(wave) + 1).ToString() + " / " + waves.Count.ToString();
            }
		}
	}

    IEnumerator generateEnemy(Wave wave)
	{
        for(int i = 0; i < wave.amounts; ++i)
		{
			pooler.PoolNamedObject(wave.enemyObject.name, true);
			yield return new WaitForSeconds(wave.createInterval);
        }
        wave.started = true;
	}
}
