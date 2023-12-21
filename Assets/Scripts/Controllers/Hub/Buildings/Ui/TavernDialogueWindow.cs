using Assets.Scripts.Helpers;
using System.Linq;
using UnityEngine;

public class TavernDialogueWindow : DialogueBox
{
    public GameObject economicsWindowPrefab;
    public GameObject skillTreeWindowPrefab;
    public GameObject inventoryWindowPrefab;

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

}
