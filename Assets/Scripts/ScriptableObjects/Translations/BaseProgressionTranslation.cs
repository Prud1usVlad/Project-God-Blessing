using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProgressionTranslation : ScriptableObject
{
    public int currentPoints = 0;
    public int currentLevelIdx = 0;
    public int requiredForNextLevel => GetPointsForNextLevel();
    public bool atLevelCap { get; protected set; } = false;

    public abstract bool AddPoints(int amount);
    public abstract void SetLevel(int level);
    public abstract void SetPoints(int amount);

    public abstract int GetPointsForNextLevel();
}