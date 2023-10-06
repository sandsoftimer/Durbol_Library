using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public partial class GameplayData
{
    public int gameScore;
    public int currentLevelNumber;

    public float gameStartTime, gameEndTime;
    public float totalLevelCompletedTime;

    public ulong totalCoin;

    public bool gameoverSuccess;
    public string weaponIds;

    public GameplayData()
    {
        gameScore = 0;
        currentLevelNumber = 0;

        gameStartTime = 0f;
        gameEndTime = 0f;
        totalLevelCompletedTime = 0f;

        weaponIds = string.Empty;
        gameoverSuccess = false;
    }
}

public class EditorButtonStyle
{
    public Color buttonColor;
    public Color buttonTextColor;
}
