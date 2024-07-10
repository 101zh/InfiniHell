using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    public void OnStartButtonPress()
    {
        Debug.Log("Load Main Game");
        SceneManager.LoadScene("MainScene");
    }
    public void OnQuitButtonPress()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
    public void OnOptionsButtonPress()
    {
        Debug.Log("this would open the options menu if I created it");
    }
}
