using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deathMenu;

    private InfiniHellInput inputActions;
    private InputAction Esc;
    private bool paused = false;
    private bool dead = false;

    void Update()
    {
        if (Esc.triggered && !dead)
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
        inputActions = new InfiniHellInput();
    }

    private void OnEnable()
    {
        Esc = inputActions.UI.Pause;
        Esc.Enable();
    }

    private void OnDisable()
    {
        Esc.Disable();
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
