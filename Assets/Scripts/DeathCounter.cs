using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    public Text deathCounterText;
    public int totalDeaths = 0;

    // Update is called once per frame
    void Update()
    {
        deathCounterText.text = "Deaths: " + totalDeaths;
    }
}
