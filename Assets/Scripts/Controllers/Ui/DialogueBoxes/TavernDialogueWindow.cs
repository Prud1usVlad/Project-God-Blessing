using Assets.Scripts.Helpers;
using System.Linq;
using UnityEngine;

public class TavernDialogueWindow : DialogueBox
{
    public GameObject economicsWindowPrefab;
    public GameObject skillTreeWindowPrefab;

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
        EndDialogue();
    }

    public void OnEconomics()
    {
        Instantiate(economicsWindowPrefab, transform);
    }

    public void OnInventory()
    {
        Debug.Log("Inventory");
    }

    public void OnResearch(int nationIdx) 
    {
        Instantiate(skillTreeWindowPrefab, transform)
            .GetComponent<SkillTreeWindow>()
            .InitWindow((NationName)nationIdx);
    }

}
