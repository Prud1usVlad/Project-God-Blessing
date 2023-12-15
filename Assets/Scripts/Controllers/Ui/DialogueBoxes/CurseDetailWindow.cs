using TMPro;
using UnityEngine;

internal class CurseDetailWindow : DialogueBox
{
    public TextMeshProUGUI curse;
    public TextMeshProUGUI prophesy;
    public Transform imgParent;
    public Transform modsParent;

    public GameObject modifierPref;

    public void InitData(CurseCard curseCard)
    {
        curse.SetText(curseCard.curseName);
        prophesy.SetText(curseCard.prophesy);

        Instantiate(curseCard.image, imgParent);

        foreach(var resMod in curseCard.modifiers.statModifiers)
        {
            Instantiate(modifierPref, modsParent)
                .GetComponent<ModifierWidget>()
                .UpdateView(resMod);
        }

        foreach (var statMod in curseCard.modifiers.statModifiers)
        {
            Instantiate(modifierPref, modsParent)
                .GetComponent<ModifierWidget>()
                .UpdateView(statMod);
        }

    }

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        return inited;
    }

    public void OnClose()
    {
        modalManager.DialogueClose();
    }
}
