using Assets.Scripts.Helpers.Enums;
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

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void InitDialogue() 
    {
        gameObject.SetActive(true);

        headerSection.SetText(header);
        bodySection.SetText(body);

        foreach (var button in buttons)
        {
            var btnObj = Instantiate(button.gameObject, buttonsSection.transform);
            var btn = btnObj.GetComponent<Button>();
            var dialogueBtn = btnObj.GetComponent<DialogueBtn>();

            btn.onClick.AddListener(() => { result = dialogueBtn.result; });
            btn.onClick.AddListener(EndDialogue);
        }
    }

    protected virtual void EndDialogue()
    {
        gameObject.SetActive(false);

        Invoke(nameof(DestroyDialogue), 5);
    }

    protected virtual void DestroyDialogue()
    {
        Destroy(gameObject);
    }
}
