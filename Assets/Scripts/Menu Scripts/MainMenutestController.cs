using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenutestController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }

    public void OpenControlsinfo()
    {
        SceneManager.LoadScene("ControlsInfo");
    }

    public void OpenOptions()
    {
        Application.Quit();
    }
    public void ExitGame(){
        Debug.Log("Quit");
        Application.Quit();
    }
}
