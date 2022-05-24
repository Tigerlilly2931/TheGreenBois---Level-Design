using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartDaGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitDaGame()
    {
        Application.Quit();
    }
}
