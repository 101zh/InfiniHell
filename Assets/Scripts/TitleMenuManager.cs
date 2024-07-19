using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject recordsMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private TMP_Text statsText;

    private void Start()
    {
        statsText.text = $"Longest Time Survived ★:\n{PlayerPrefs.GetFloat("BestTime", 0.0f)}\nHighest Difficulty Reached ★:\n{PlayerPrefs.GetFloat("BestDiff", 0.0f)}";
    }

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
    public void OnMenuChangeButtonPress(GameObject menu)
    {
        titleMenu.SetActive(false);
        menu.SetActive(true);
    }

    public void OnTutorialButtonPress()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void onBackButtonPressed()
    {
        recordsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        titleMenu.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        GameManager.clearDiff();
    }
}
