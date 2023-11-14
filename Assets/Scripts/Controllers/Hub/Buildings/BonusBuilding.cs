using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Bonus", fileName = "Building")]
public class BonusBuilding : Building
{
    public ModifiersContainer modifiers;

    public override void InitDialogue(DialogueBox dialogueBox)
    {
        (dialogueBox as BonusBuildingDialogueBox).building = this;
        dialogueBox.InitDialogue();
    }

    private void OnEnable()
    {
        modifiers?.InitSource(this);  
    }
}
