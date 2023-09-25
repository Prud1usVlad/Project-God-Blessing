using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Bonus", fileName = "Building")]
public class BonusBuilding : Building
{
    List<StatModifier> modifiers;

    public override void InitDialogue(DialogueBox dialogueBox)
    {
        throw new System.NotImplementedException();
    }
}
