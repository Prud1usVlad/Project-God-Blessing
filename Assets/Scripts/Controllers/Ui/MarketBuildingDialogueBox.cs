using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketBuildingDialogueBox : DialogueBox
{
    public MarketBuilding building;

    public override void InitDialogue()
    {
        header = building.buildingName;
        body = building.description;

        base.InitDialogue();
        UpdateView();

    }

    public void UpdateView()
    {
    }
}
