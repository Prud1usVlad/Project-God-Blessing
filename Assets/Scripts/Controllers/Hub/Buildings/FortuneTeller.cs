using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/FortuneTeller", fileName = "FortuneTeller")]
public class FortuneTeller : Building
{
    public override void InitDialogue(DialogueBox dialogueBox, BuildingController controller = null)
    {
        modalManager.DialogueOpen(dialogueBox);
    }
}

