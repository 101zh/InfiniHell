using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private TMP_Text controlsInfoText;
    [SerializeField] private bool tutorial = false;

    private bool paused = false;
    private bool dead = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !dead)
        {
            if (!paused)
            {
                pause();
            }
            else
            {
                unpause();
            }
        }
    }

    private void Awake()
    {
        string controlScheme = PlayerPrefs.GetString("Controls", GameManager.defaultControls);
        if (tutorial)
            controlsInfoText.text = "Use " + controlScheme;
    }
    public void onTitleButtonPressed()
    {
        GameManager.clearDiff();
        unpause();
        SceneManager.LoadScene("TitleScene");
    }

    public void onRetryFromStartPressed()
    {
        GameManager.clearDiff();
        unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onRetryDifficulty()
    {
        unpause();
        GameManager.setDiff(gameManager.getDifficultyOnDeath());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onQuitButtonPressed()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        paused = true;
    }
    public void unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }

    private void OnApplicationQuit()
    {
        GameManager.clearDiff();
    }

    public void activateDeathMenu()
    {
        dead = true;
        deathMenu.SetActive(true);
    }
}
