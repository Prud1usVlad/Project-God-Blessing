using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Market", fileName = "Building")]
public class MarketBuilding : Building
{
    public NationName nation;
    public int daysToUpdate;

    public EquipmentItemRegistry itemRegistry;

    public override void InitDialogue(DialogueBox dialogueBox)
    {
        (dialogueBox as MarketBuildingDialogueBox).building = this;
        dialogueBox.InitDialogue();
    }
}
