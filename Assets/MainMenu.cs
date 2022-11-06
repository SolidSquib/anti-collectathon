using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string _SceneToLoad = "";

    public void LoadScene ()
    {
        SceneManager.LoadScene(_SceneToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
