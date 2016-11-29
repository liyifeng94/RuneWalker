using System;
using UnityEngine;
using System.Collections;

public class SimpleUI : MonoBehaviour
{
    void OnGUI()
    {
        string levelText = "Level: ";
        int level = GameManager.Instance.GetCurrentLevel();
        levelText += level.ToString();
        GUI.Label(new Rect(10, 10, 100, 20), levelText);

        string scoreText = "Score: ";
        int score = GameManager.Instance.GetScore();
        scoreText += score.ToString();
        GUI.Label(new Rect(10, 40, 100, 20), scoreText);


        string multiplierText = "Multiplier: x";
        int multiplier = GameManager.Instance.GetScoreMultiplier();
        multiplierText += multiplier.ToString();
        GUI.Label(new Rect(10, 70, 100, 20), multiplierText);

        string specialText = "Special: ";
        int special = GameManager.Instance.GetSpecialMeter();
        int specialMax = GameManager.Instance.GetSpecialMeterMax();
        special = Math.Min(special, specialMax);
        int specialMeter = (int)(((double)special/(double)specialMax) * 100);
        specialText += specialMeter.ToString() + "%";
        GUI.Label(new Rect(10, 100, 100, 20), specialText);

    }
}
