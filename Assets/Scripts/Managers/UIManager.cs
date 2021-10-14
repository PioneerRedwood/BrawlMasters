using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Other Manager Handle")]
    public Text weaponStatText;
    public Text waveText;

    [Header("FPS")]
    // https://docs.unity3d.com/ScriptReference/Time-realtimeSinceStartup.html
    public bool isCheckFps;
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames;
    private float fps;

    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    void OnGUI()
    {
        GUILayout.Label("" + fps.ToString("f2"));
    }

    [Header("Timer")]
    public Text timerText;
    public float passedTime;
    
    //private float passedMinutes;
    //private float passedSeconds;

    void Update()
    {
        if(isCheckFps)
		{
            ++frames;
            float timeNow = Time.realtimeSinceStartup;
            if (timeNow > lastInterval + updateInterval)
            {
                fps = (float)(frames / (timeNow - lastInterval));
                frames = 0;
                lastInterval = timeNow;
            }
        }
        passedTime += Time.deltaTime;
        DisplayTime(passedTime);

        //passedMinutes = Mathf.FloorToInt(passedTime / 60);
        //passedSeconds = Mathf.FloorToInt(passedTime % 60);
        //timerText.text = string.Format("{0:00}:{1:00}", passedMinutes, passedSeconds);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
