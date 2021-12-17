using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;

    [SerializeField]
    private float gameDuration;

    private float timeElapsed;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        gameDuration -= Time.deltaTime;
        if (gameDuration <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public static string GetFormattedTime()
    {
        // https://stackoverflow.com/questions/463642/what-is-the-best-way-to-convert-seconds-into-hourminutessecondsmilliseconds/41799528
        TimeSpan t = TimeSpan.FromSeconds(instance.gameDuration);
        if (t.Days > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",
                t.Days,
                t.Hours,
                t.Minutes,
                t.Seconds
                );
        }
        else if (t.Hours > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds
                );
        }
        else
        {
            return string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds
                );
        }
    }
}
