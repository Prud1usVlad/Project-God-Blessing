using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TavernDialogueWindow : DialogueBox
{
    [SerializeField]
    private List<NationFiller> fillers;

    public GameObject economicsWindowPrefab;
    public GameObject skillTreeWindowPrefab;
    public GameObject inventoryWindowPrefab;

    public GameProgress gameProgress;

    public override bool InitDialogue()
    {
        header = "Tavern";
        body = "Main cabinet of your new empire";

        var inited = base.InitDialogue();

        if (inited)
        {
            UpdateView();
        }

        return inited;
    }

    public void UpdateView()
    {
        var connections = gameProgress.skillSystem.connections;

        foreach (var f in fillers)
        {
            var conn = connections.GetConnection(f.nation);
            f.filler.fillAmount = (float)conn.currentLevelIdx / conn.levelsAmount;
        }

    }

    public void CloseWindow()
    {
        modalManager.DialogueClose();
    }

    public void OnEconomics()
    {
        var dialogue = Instantiate(economicsWindowPrefab, transform)
            .GetComponent<EconomicsWindow>();

        modalManager.DialogueOpen(dialogue);
    }

    public void OnInventory()
    {
        var dialogue = Instantiate(inventoryWindowPrefab, transform)
            .GetComponent<InventoryWindow>();

        modalManager.DialogueOpen(dialogue);
    }

    public void OnResearch(int nationIdx) 
    {
        var dialogue = Instantiate(skillTreeWindowPrefab, transform)
            .GetComponent<SkillTreeWindow>();

        dialogue.InitData((NationName)nationIdx);

        modalManager.DialogueOpen(dialogue);
    }

    [Serializable]
    private class NationFiller
    {
        public NationName nation;
        public Image filler;
    }

}
