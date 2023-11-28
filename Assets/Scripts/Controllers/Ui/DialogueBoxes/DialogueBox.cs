using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.ScriptableObjects.Hub;
using Assets.Scripts.TooltipSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : TooltipDataProvider
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

    public bool discardOtherWhileOpen = false;
    public bool allowInBuildMode = false;
    public bool ignoreConstraints = false;
    public bool requireResultToClose = false;

    public ModalManager modalManager;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual bool InitDialogue() 
    {
        if (ignoreConstraints)
            Init();
        else if (modalManager.isDiscarding)
        {
            DestroyDialogue();
            return false;
        }
        else
        {
            Init();
        }

        return true;
    }

    public virtual void EndDialogue()
    {
        //if (!ignoreConstraints)
        //runtimeData?.DialogueClose();

        if (requireResultToClose && result == DialogueBoxResult.None)
            return;

        if (discardOtherWhileOpen)
            modalManager.isDiscarding = false;

        gameObject.SetActive(false);

        Invoke(nameof(DestroyDialogue), 2);
    }

    protected virtual void DestroyDialogue()
    {
        Destroy(gameObject);
    }

    public override string GetHeader(string tag = null)
    {
        return header;
    }

    public override string GetContent(string tag = null)
    {
        return body;
    }

    private void Init()
    {
        gameObject.SetActive(true);

        headerSection?.SetText(header);
        bodySection?.SetText(body);

        foreach (var button in buttons)
        {
            var btnObj = Instantiate(button.gameObject, buttonsSection.transform);
            var btn = btnObj.GetComponent<Button>();
            var dialogueBtn = btnObj.GetComponent<DialogueButton>();

            btn.onClick.AddListener(() => result = dialogueBtn.result);
            btn.onClick.AddListener(() => modalManager.DialogueClose());
        }

        modalManager.isDiscarding = discardOtherWhileOpen;
    }
}
