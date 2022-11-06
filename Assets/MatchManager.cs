using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public string _EndScenename = "";

    public void EndMatch()
    {
        SceneManager.LoadScene(_EndScenename);
    }
}
