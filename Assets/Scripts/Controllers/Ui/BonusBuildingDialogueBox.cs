using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BonusBuildingDialogueBox : DialogueBox
{
    private List<ModifierWidget> widgetsR;
    private List<ModifierWidget> widgetsS;

    public BonusBuilding building;
    public GameObject widgetPrefab;

    public Transform resModsArea;
    public Transform statModsArea;

    public override void InitDialogue()
    {
        header = building.buildingName;
        body = building.description;
        widgetsR = new List<ModifierWidget>();
        widgetsS = new List<ModifierWidget>();

        base.InitDialogue();
        UpdateView();

    }

    public void UpdateView()
    {
        if (widgetsS.Count == 0)
            InitWidgets(widgetsS, statModsArea, building.statModifiers.Count);
        if (widgetsR.Count == 0) 
            InitWidgets(widgetsR, resModsArea, building.resourceModifiers.Count);

        UpdateWidgets(widgetsS, building.statModifiers.Cast<object>().ToList());
        UpdateWidgets(widgetsR, building.resourceModifiers.Cast<object>().ToList());
    }

    private void InitWidgets(List<ModifierWidget> wList, Transform parent, int amount)
    {
        if (wList.Count == 0)
        {
            for (int i = 0; i < amount; i++)
            {
                var widget = Instantiate(widgetPrefab, parent)
                .GetComponent<ModifierWidget>();

                wList.Add(widget);
            }
        }
    }

    private void UpdateWidgets(List<ModifierWidget> wList, List<object> data)
    {
        for(int i = 0; i < wList.Count; i++)
        {
            var d = data[i];
            if (d is ResMod)
                wList[i].UpdateView(d as ResMod);
            else if (d is StatMod)
                wList[i].UpdateView(d as StatMod);
        }
    }
}
