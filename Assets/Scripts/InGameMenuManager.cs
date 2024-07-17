using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    public void onTitleButtonPressed()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void onRetryFromStartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onRetryDifficulty()
    {
        Debug.Log("Not Yet Implemented");
    }

    public void onQuitButtonPressed()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
