using Assets.Scripts.SkillSystem;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "ScriptableObjects/Translations/Connection", fileName = "ConnectionTranslation")]
public class ConnectionsTranslation : BaseProgressionTranslation
{
    public NationName nation;

    public float pointsMultiplier = 100;
    public int levelsAmount;

    public int aquiredResearchPoints;
    public int freeResearchPoints;

    [Tooltip("Amount of points that is considered still first level, exclusive")]
    public int firstLevelCap = 5;
    [Tooltip("Amount of points that is considered still second level, exclusive")]
    public int secondLevelCap = 15;

    public int pointsCap => GetLevelPoints(levelsAmount);
    public int connectionLevel => 
        currentLevelIdx < firstLevelCap ? 1 : 
        currentLevelIdx < secondLevelCap ? 2 : 3; 

    public override bool AddPoints(int amount)
    {
        if (atLevelCap)
            return false;

        currentPoints += amount;
        currentPoints = Mathf.Min(currentPoints, pointsCap);

        var oldLevel = currentLevelIdx;
        currentLevelIdx = GetLevel(currentPoints);

        atLevelCap = levelsAmount == currentLevelIdx;

        var levelChanged = oldLevel != currentLevelIdx;
        if (levelChanged)
        {
            if (currentLevelIdx >  aquiredResearchPoints) 
            {
                aquiredResearchPoints = currentLevelIdx;
                freeResearchPoints++;
            }
        }

        return levelChanged;
    }

    public override int GetPointsForNextLevel()
    {
        if (atLevelCap)
            return int.MaxValue;

        return GetLevelPoints(currentLevelIdx + 1) - currentPoints;
    }

    public override void SetLevel(int level)
    {
        if (level < 0 || level > levelsAmount)
            throw new ArgumentOutOfRangeException($"Level index is {level}," +
                $" expected in range from 0 to {levelsAmount}");

        SetPoints(GetLevelPoints(level));
    }

    public override void SetPoints(int amount)
    {
        currentPoints = Mathf.Min(amount, pointsCap);
        currentLevelIdx = GetLevel(amount);
    }

    public void UseResearchPoint(Skill skill)
    {
        if (freeResearchPoints >= skill.pointsRequired)
        {
            freeResearchPoints -= skill.pointsRequired;
        }
    }

    public bool CanLearn(Skill skill)
    {
        return skill.level <= connectionLevel 
            && skill.pointsRequired <= freeResearchPoints;
    }

    public int GetLevelPoints(int reqLevel)
    {
        if (reqLevel < 0) return 0;

        float sum = 0;
        for (var i = 1; i <= reqLevel; i++)
        {
            sum += Mathf.Log(i + 1, 2) * pointsMultiplier;
        }

        return Mathf.RoundToInt(sum);
    }
    
    private int GetLevel(int points)
    {
        var i = 0;
        for (; i <= levelsAmount; i++)
        {
            if (points < GetLevelPoints(i))
                break;
        }

        return i;
    }
}