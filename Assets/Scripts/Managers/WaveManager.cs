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
            }
		}
	}

    // 2021-10-14 지정한 인터벌마다 실행하는 것은 확인했으나
    // pooler에서 지정한 함수를 호출하는데 제대로 생성이 안됨 해결 필요
    IEnumerator generateEnemy(Wave wave)
	{
        for(int i = 0; i < wave.amounts; ++i)
		{
			pooler.PoolNamedObject(wave.enemyObject.name);
			//Debug.Log($"Generate {UIManager.Instance.passedTime % 60}");
			yield return new WaitForSeconds(wave.createInterval);
        }
        wave.started = true;
	}
}
