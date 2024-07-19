using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public void onTitleButtonPressed()
    {
        GameManager.clearDiff();
        SceneManager.LoadScene("TitleScene");
    }

    public void onRetryFromStartPressed()
    {
        GameManager.clearDiff();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onRetryDifficulty()
    {
        GameManager.setDiff(gameManager.getCurDifficulty());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onQuitButtonPressed()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        GameManager.clearDiff();
    }
}
