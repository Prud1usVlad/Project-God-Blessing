using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/Translations/Lies", fileName = "LiesTranslation")]
public class LiesTranslation : BaseProgressionTranslation
{
    public CurseRegistry curseRegistry;

    public int levelsAmount;
    public int pointsForLevel;
    public int pointsCap => levelsAmount * pointsForLevel;

    public GameProgress gameProgress;

    public override bool AddPoints(int amount)
    {
        if (atLevelCap)
            return false;

        currentPoints += amount;
        currentPoints = Mathf.Min(currentPoints, pointsCap);

        var oldLevel = currentLevelIdx;
        currentLevelIdx = (int)Mathf.Floor(currentPoints / pointsForLevel);

        atLevelCap = levelsAmount == currentLevelIdx;

        var levelChanged = oldLevel != currentLevelIdx;
        if (levelChanged)
        {
            var curse = curseRegistry.GetRandom(currentLevelIdx);
            gameProgress.curses.Add(curse);
            gameProgress.globalModifiers.AddModifiers(curse);
        }
            

        return levelChanged; 
    }
    
    public bool RemovePoints(int amount)
    {
        currentPoints -= amount;
        currentPoints = Mathf.Max(currentPoints, 0);

        var oldLevel = currentLevelIdx;
        currentLevelIdx = (int)Mathf.Floor(currentPoints / pointsForLevel);

        atLevelCap = levelsAmount == currentLevelIdx;

        var levelChanged = oldLevel != currentLevelIdx;
        if (levelChanged)
        {
            var curse = gameProgress.curses.Last();
            gameProgress.curses.Remove(curse);
            gameProgress.globalModifiers.RemoveModifiers(curse);
        }
            

        return levelChanged;
    }

    public override int GetPointsForNextLevel()
    {
        if (atLevelCap)
            return int.MaxValue;

        return currentPoints % pointsForLevel;
    }

    public override void SetLevel(int level)
    {
        if (level < 0 || level > levelsAmount)
            throw new ArgumentOutOfRangeException($"Level index is {level}," +
                $" expected in range from 0 to {levelsAmount}");

        currentLevelIdx = level;
        AddPoints(level * pointsForLevel);
    }

    public override void SetPoints(int amount)
    {
        currentPoints = amount;
        currentPoints = Mathf.Min(currentPoints, pointsCap);

        currentLevelIdx = (int)Mathf.Floor(currentPoints / pointsForLevel);

        atLevelCap = levelsAmount == currentLevelIdx;
    }
}
