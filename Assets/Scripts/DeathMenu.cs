using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text statsText;

    private void OnEnable()
    {
        statsText.text = $"Time Survived: {gameManager.getTimeSurvived()}\nDifficulty Reached: {gameManager.getCurDifficulty()}";
    }
}
