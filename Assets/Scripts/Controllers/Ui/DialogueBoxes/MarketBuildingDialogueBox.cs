using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketBuildingDialogueBox : DialogueBox
{
    public MarketBuilding building;

    public override bool InitDialogue()
    {
        header = building.buildingName;
        body = building.description;

        var inited = base.InitDialogue();
        
        if (inited) UpdateView();
        return inited;
    }

    public void UpdateView()
    {
    }
}
