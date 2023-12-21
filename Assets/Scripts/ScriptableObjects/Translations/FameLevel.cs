using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;

[CreateAssetMenu(menuName = "ScriptableObjects/FameLevel", fileName = "FameLevel")]
public class FameLevel : SerializableScriptableObject
{
    public string levelName;
    public string description;
    public int points;

    public ModifiersContainer modifiers;

    private void OnEnable()
    {
        modifiers?.InitSource(this);
    }
}
