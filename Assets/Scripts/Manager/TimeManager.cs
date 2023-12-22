using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public TextMeshProUGUI text;
    public float time;

    private void Awake()
    {
        instance = this;
        time = 60f;
    }
    void Update()
    {
        TimeCount();

        if (time > 0)
        {
            if (time <= 10)
            {
                ColorChange();
            }
        }
        else
            TimeStop();
    }

    public void TimeCount()
    {
        time -= Time.deltaTime;
        text.text = string.Format("{0}{1}", "Time left: ", Mathf.Ceil(time).ToString());
    }

    public void TimeStart()
    {
        Time.timeScale = 1;
    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }

    public void BreakTime()
    {
        time = 5f;    
    }

    public void RoundTime()
    {
        time = 60f;
    }

    public void ColorChange()
    {
        text.color = Color.red;
    }
}

