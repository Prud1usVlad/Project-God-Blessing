using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/FortuneTeller", fileName = "FortuneTeller")]
public class FortuneTeller : Building
{
    public GameProgress gameProgress;

    public override void InitDialogue(DialogueBox dialogueBox)
    {
        throw new System.NotImplementedException();
    }
}

