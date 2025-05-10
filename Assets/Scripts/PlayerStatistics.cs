using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public struct LevelInfo
{
    public bool completed;
    public double levelTime;
    public int hintsRequested;

    public LevelInfo(bool _completed = false, double _levelTime = 0, int _hintsRequested = 0)
    {
        completed = _completed;
        levelTime = _levelTime;
        hintsRequested = _hintsRequested;
    }
}

public static class PlayerStatistics
{
    public static Dictionary<string, LevelInfo> levels = new Dictionary<string, LevelInfo>();
    private static Stopwatch timer;
    private static string currentLevel = "";

    public static void EnterLevel(string levelName)
    {
        currentLevel = levelName;
        if (!levels.ContainsKey(levelName)) levels.Add(levelName, new LevelInfo());

        LevelInfo thisLevel = levels[levelName];
        if (!thisLevel.completed) StartLevelTimer(levelName);
    }

    public static void CompeltePuzzleLevel()
    {
        if (currentLevel == "") return;
        LevelInfo thisLevel = levels[currentLevel];
        thisLevel.completed = true;
        levels[currentLevel] = thisLevel;
    }

    public static void StartLevelTimer(string levelName)
    {
        timer = new Stopwatch();
        timer.Start();
    }

    public static void StopLevelTimer()
    {
        if (timer != null && currentLevel != "")
        {
            timer.Stop();
            LevelInfo thisLevel = levels[currentLevel];
            thisLevel.levelTime += timer.Elapsed.TotalSeconds;
            levels[currentLevel] = thisLevel;
        }       
    }

    public static void HintUsed()
    {
        if (currentLevel == "") return;
        LevelInfo thisLevel = levels[currentLevel];
        ++thisLevel.hintsRequested;
        levels[currentLevel] = thisLevel;
    }
}
