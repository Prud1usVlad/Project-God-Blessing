using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Tavern", fileName = "Tavern")]
public class Tavern : Building
{
    public override void InitDialogue(DialogueBox dialogueBox)
    {
        dialogueBox.InitDialogue();
    }
}
