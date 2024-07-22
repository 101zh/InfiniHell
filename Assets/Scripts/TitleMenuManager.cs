using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject recordsMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private TMP_Dropdown controlsDropDown;

    private void Start()
    {
        statsText.text = $"Longest Time Survived ★:\n{PlayerPrefs.GetFloat("BestTime", 0.0f)}\nHighest Difficulty Reached ★:\n{PlayerPrefs.GetFloat("BestDiff", 0.0f)}";
        controlsDropDown.value = initControlsDropDown();
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

    public void setControlsDropDown(int index)
    {
        switch (index)
        {
            case 0:
                PlayerPrefs.SetString("Controls", "WASD"); break;
            case 1:
                PlayerPrefs.SetString("Controls", "Arrows"); break;
            case 2:
                PlayerPrefs.SetString("Controls", "Mouse"); break;
            case 3:
                PlayerPrefs.SetString("Controls", "LeftStick"); break;
            case 4:
                PlayerPrefs.SetString("Controls", "OnScreen"); break;
        }
    }

    private int initControlsDropDown()
    {
        string controls = PlayerPrefs.GetString("Controls", GameManager.defaultControls);

        switch (controls)
        {
            case "WASD":
                return 0;
            case "Arrows":
                return 1;
            case "Mouse":
                return 2;
            case "LeftStick":
                return 3;
            case "OnScreen":
                return 4;
        }

        return -1;
    }
}
