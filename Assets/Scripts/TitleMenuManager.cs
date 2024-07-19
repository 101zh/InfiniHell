using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject optionsMenu;

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
        titleMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OnTutorialButtonPress()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void onBackButtonPressed()
    {
        optionsMenu.SetActive(false);
        titleMenu.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        GameManager.clearDiff();
    }
}
