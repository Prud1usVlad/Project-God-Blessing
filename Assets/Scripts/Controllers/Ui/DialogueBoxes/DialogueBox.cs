using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.ScriptableObjects.Hub;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [NonSerialized]
    public string header;
    [NonSerialized]
    public string body;

    public List<GameObject> buttons;

    [NonSerialized]
    public DialogueBoxResult result = DialogueBoxResult.None;

    public TextMeshProUGUI headerSection;
    public TextMeshProUGUI bodySection;
    public GameObject buttonsSection;

    public RuntimeHubUiData runtimeData;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual bool InitDialogue() 
    {
        if (runtimeData.isDialogOpened)
        {
            DestroyDialogue();
            return false;
        }

        runtimeData.DialogueOpen(this);
        gameObject.SetActive(true);

        headerSection?.SetText(header);
        bodySection?.SetText(body);

        foreach (var button in buttons)
        {
            var btnObj = Instantiate(button.gameObject, buttonsSection.transform);
            var btn = btnObj.GetComponent<Button>();
            var dialogueBtn = btnObj.GetComponent<DialogueButton>();

            btn.onClick.AddListener(() => { result = dialogueBtn.result; });
            btn.onClick.AddListener(EndDialogue);
        }

        return true;
    }

    protected virtual void EndDialogue()
    {
        runtimeData.DialogueClose();
        gameObject.SetActive(false);

        Invoke(nameof(DestroyDialogue), 2);
    }

    protected virtual void DestroyDialogue()
    {
        Destroy(gameObject);
    }
}
