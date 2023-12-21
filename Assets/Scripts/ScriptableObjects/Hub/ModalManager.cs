using Assets.Scripts.Helpers.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Hub
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ModalManager", fileName = "ModalManager")]
    public class ModalManager : ScriptableObject
    {
        public bool isDialogOpened = false;
        public bool isInBuildMode = false;
        public bool isDiscarding = false;
        public Stack<DialogueBox> modals;

        public void DialogueOpen(DialogueBox dialogueBox)
        {
            if (dialogueBox.InitDialogue())
            {
                modals.Push(dialogueBox);
                isDialogOpened = true;
            }
        }

        public DialogueBoxResult DialogueClose()
        {
            if (isDialogOpened && modals.Count > 0) 
            {
                var modal = modals.Pop();
                modal.EndDialogue();

                isDialogOpened = modals.Count > 0;
                return modal.result; 
            }
            else
            {
                isDialogOpened = modals.Count > 0;
                return DialogueBoxResult.None;
            }
        }

        private void OnEnable()
        {
            isDialogOpened = false;
            isInBuildMode = false;
            isDiscarding = false;
            modals = new();
        }
    }
}
