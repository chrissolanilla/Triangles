using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class ReportGeneration : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelNamesText;
    [SerializeField]
    private TextMeshProUGUI levelTimesText;
    [SerializeField]
    private TextMeshProUGUI levelHintsText;
    [SerializeField]
    private TextMeshProUGUI levelCompletionText;

    void Start()
    {
        string names = "";
        string hints = "";
        string times = "";
        string completion = "";

        foreach (KeyValuePair<string, LevelInfo> level in PlayerStatistics.levels)
        {
            names += level.Key + "\n";
            times += SecondsToString(level.Value.levelTime) + "\n";
            if (level.Key == "Whiteboard")
            {
                hints += "N/A\n";
                completion += "N/A\n";
            }
            else 
            {
                hints += level.Value.hintsRequested.ToString() + "\n";
                completion += BoolToString(level.Value.completed) + "\n";
            }
        }

        levelNamesText.text = names;
        levelTimesText.text = times;
        levelHintsText.text = hints;
        levelCompletionText.text = completion;
    }

    string SecondsToString(double totalSeconds)
    {
        int hours = Mathf.FloorToInt((float)totalSeconds / 3600);
        int minutes = Mathf.FloorToInt((float)totalSeconds / 60);
        int seconds = Mathf.FloorToInt((float)totalSeconds) % 60;
        return String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    string BoolToString(bool completed)
    {
        return (completed) ? "Yes" : "No";
    }
}
