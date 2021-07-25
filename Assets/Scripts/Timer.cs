using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;

    // Update is called once per frame
    void Update()
    {
        float seconds = (Time.timeSinceLevelLoad % 60);
        int minutes = (int)Time.timeSinceLevelLoad / 60;
        if (minutes > 0)
        {
            timerText.text = "Time: " + minutes +"."+ seconds.ToString("f2");
        }
        else
        {
            timerText.text = "Time: " + seconds.ToString("f2");
        }
    }
}
