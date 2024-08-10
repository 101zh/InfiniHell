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
        controlsDropDown.value = ControlsToInt();
    }

    public void OnStartButtonPress()
    {
        GameManager.clearDiff();
        GameManager.level = -1;
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
        GameManager.clearDiff();
        GameManager.level = 0;
        SceneManager.LoadScene("MainScene");
    }

    public void onBackButtonPressed()
    {
        recordsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        titleMenu.SetActive(true);
    }

    public void setControlsDropdown(int index)
    {
        setControls(index);
    }

    public static string setControls(int index)
    {
        switch (index)
        {
            case 0:
                PlayerPrefs.SetString("Controls", "WASD"); return "WASD";
            case 1:
                PlayerPrefs.SetString("Controls", "Arrows"); return "Arrows";
            case 2:
                PlayerPrefs.SetString("Controls", "Mouse"); return "Mouse";
            case 3:
                PlayerPrefs.SetString("Controls", "LeftStick"); return "LeftStick";
            case 4:
                PlayerPrefs.SetString("Controls", "OnScreenJoystick"); return "OnScreenJoystick";
        }

        return null;
    }

    public static int ControlsToInt()
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
            case "OnScreenJoystick":
                return 4;
        }

        return -1;
    }
}
