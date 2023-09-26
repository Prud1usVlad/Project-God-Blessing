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
    public List<StatMod> statModifiers;
    public List<ResMod> resourceModifiers;

    public override void InitDialogue(DialogueBox dialogueBox)
    {
        (dialogueBox as BonusBuildingDialogueBox).building = this;
        dialogueBox.InitDialogue();
    }
}
