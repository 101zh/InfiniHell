using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] TMP_Text statsText;

    private void OnEnable()
    {
        float timeSurvived = GameManager.getTimeSurvived();
        float diff = GameManager.getDifficultyOnDeath();

        float bestDiff = PlayerPrefs.GetFloat("BestDiff", diff);
        float bestTime = PlayerPrefs.GetFloat("BestTime", timeSurvived);

        if (diff >= bestDiff)
        {
            bestDiff = diff;
            PlayerPrefs.SetFloat("BestDiff", diff);
        }
        if (timeSurvived >= bestTime)
        {
            bestTime = timeSurvived;
            PlayerPrefs.SetFloat("BestTime", timeSurvived);
        }


        statsText.text = $"Time Survived: {GameManager.getTimeSurvived()} ★: {bestTime} \nDifficulty Reached: {GameManager.getDifficultyOnDeath()} ★: {bestDiff} ";
    }
}
