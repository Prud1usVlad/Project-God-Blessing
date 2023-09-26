using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Market", fileName = "Building")]
public class MarketBuilding : Building
{
    public override void InitDialogue(DialogueBox dialogueBox)
    {
        (dialogueBox as MarketBuildingDialogueBox).building = this;
        dialogueBox.InitDialogue();
    }
}
