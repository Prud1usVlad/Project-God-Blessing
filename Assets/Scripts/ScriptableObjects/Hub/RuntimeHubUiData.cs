using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Hub
{
    [CreateAssetMenu(menuName = "ScriptableObjects/RuntimeData/Hub/Ui", fileName = "RuntimeData")]
    public class RuntimeHubUiData : ScriptableObject
    {
        public bool isDialogOpened = false;
        public DialogueBox openedDialogue = null;

        public void DialogueOpen(DialogueBox dialogueBox)
        {
            openedDialogue = dialogueBox;
            isDialogOpened = true;
        }

        public void DialogueClose()
        {
            openedDialogue = null;
            isDialogOpened = false;
        }

        private void OnEnable()
        {
            DialogueClose();
        }
    }
}
