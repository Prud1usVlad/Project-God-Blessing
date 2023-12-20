using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Production", fileName = "Building")]
public class ProductionBuilding : Building
{
    public List<ProductionRecipe> productionRecipes;

    public override void InitDialogue(DialogueBox dialogueBox,
        BuildingController controller = null)
    {
        var box = dialogueBox as ProductionBuildingDialogueBox;

        box.building = this;
        box.buildingController = controller;
        modalManager.DialogueOpen(dialogueBox);
    }
}
