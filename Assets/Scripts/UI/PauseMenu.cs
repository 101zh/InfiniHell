using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] TMP_Text statsText;

    private void OnEnable()
    {
        statsText.text = $"Time Survived: {GameManager.getTime()}\nDifficulty Reached: {GameManager.getCurDifficulty()}";
    }
}
