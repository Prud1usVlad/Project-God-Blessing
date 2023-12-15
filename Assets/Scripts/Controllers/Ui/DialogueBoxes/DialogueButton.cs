using Assets.Scripts.Helpers.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textObj;

    public string defaultText;
    public DialogueBoxResult result;

    private void Awake()
    {
        textObj.text = defaultText;
    }

    public void SetText(string text)
    {
        this.textObj.SetText(text);
    }
}
