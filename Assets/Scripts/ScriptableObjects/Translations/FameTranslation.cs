using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Translations/Fame" , fileName = "FameTranslation")]
public class FameTranslation : BaseProgressionTranslation
{
    public FameLevelRegistry registry;
    public FameLevel currentFameLevel;

    public override bool AddPoints(int amount)
    {
        if (atLevelCap)
            return false;

        currentPoints += amount;
        for (int i = registry.count - 1; i >= 0; i--)
        {
            var entry = registry.GetByIndex(i);

            // found a matching entry
            if (currentPoints >= entry.points)
            {
                // level changed?
                if (i != currentLevelIdx)
                {
                    currentLevelIdx = i;
                    currentFameLevel = entry;

                    atLevelCap = registry.count - 1 == currentLevelIdx;

                    return true;
                }
                break;
            }
        }

        return false;
    }

    public override void SetLevel(int level)
    {
        currentPoints = 0;
        currentLevelIdx = 0;
        atLevelCap = false;

        var levelData = registry.GetByIndex(level);

        if (levelData != null)
        {
            AddPoints(levelData.points);
        }
        else
        {
            throw new ArgumentOutOfRangeException($"Could not find any entry for level {level}");
        }
    }

    public override void SetPoints(int amount)
    {
        currentPoints = amount;
        currentLevelIdx = 0;
        atLevelCap = false;

        for (int i = 0; i < registry.count; i++)
        {
            var level = registry.GetByIndex(i);
            if (level.points <= amount)
                currentLevelIdx = i;
            else
                break;
        }

        currentFameLevel = registry.GetByIndex(currentLevelIdx);
        atLevelCap = currentLevelIdx == registry.count - 1;

    }

    public override int GetPointsForNextLevel()
    {
        if (atLevelCap)
            return int.MaxValue;

        return registry.GetByIndex(currentLevelIdx + 1).points - currentPoints;
    }

    private void OnEnable()
    {
        if (registry != null)
            currentFameLevel = registry.GetByIndex(currentLevelIdx);
    }
}
