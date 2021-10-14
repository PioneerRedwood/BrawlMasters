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

    // 2021-10-14 ������ ���͹����� �����ϴ� ���� Ȯ��������
    // pooler���� ������ �Լ��� ȣ���ϴµ� ����� ������ �ȵ� �ذ� �ʿ�
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
