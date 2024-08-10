using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;

    private bool paused = false;
    private bool dead = false;

    private void Awake()
    {
        SceneManager.activeSceneChanged += onSceneChanged;
    }

    private void onSceneChanged(Scene current, Scene next)
    {
        unpause();
    }

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

    public void onTitleButtonPressed()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void onRetryFromStartPressed()
    {
        GameManager.clearDiff();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onRetryDifficulty()
    {
        GameManager.setDiff(GameManager.getDifficultyOnDeath());
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
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }

    public void activateDeathMenu()
    {
        dead = true;
        deathMenu.SetActive(true);
    }
}
