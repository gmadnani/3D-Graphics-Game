using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class controlsinfoController : MonoBehaviour
{

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MainMenutest");
    }
}

