using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] TMP_Text statsText;
    [SerializeField] GameManager gameManager;

    private void OnEnable()
    {
        statsText.text = $"Time Survived: {gameManager.getTime()}\nDifficulty Reached: {gameManager.getCurDifficulty()}";
    }
}
