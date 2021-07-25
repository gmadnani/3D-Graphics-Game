using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowWinScreen : MonoBehaviour
{
    public GameObject winText;
    public GameObject playAgainButton;
    public GameObject stats;
    public GameObject crosshair;
    public GameObject time;
    public GameObject deathCounter;

    private void OnTriggerEnter(Collider other)
    {
        Text timeText = time.GetComponent<Text>();
        Text deathText = deathCounter.GetComponent<Text>();
        Cursor.lockState = CursorLockMode.Confined;
        crosshair.SetActive(false);
        time.SetActive(false);
        deathCounter.SetActive(false);
        winText.SetActive(true);
        playAgainButton.SetActive(true);
        stats.GetComponent<Text>().text = timeText.text + "\n" + deathText.text;
        stats.SetActive(true);
    }
}
