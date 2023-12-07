using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Production", fileName = "Building")]
public class ProductionBuilding : Building
{
    public Resource resource;
    [Tooltip("Price for resource production on power 1. Affected by production power")]
    public Price productionPrice;
    public float productionMultiplier;

    [Range(0, 4)]
    public int productionPower = 0;

    public override void InitDialogue(DialogueBox dialogueBox,
        BuildingController controller = null)
    {
        var box = dialogueBox as ProductionBuildingDialogueBox;

        box.building = this;
        box.buildingController = controller;
        modalManager.DialogueOpen(dialogueBox);
    }

    public int GetProductionAmount()
    {
        return Mathf.RoundToInt(resource.amount
            * productionMultiplier * productionPower);
    }
}
