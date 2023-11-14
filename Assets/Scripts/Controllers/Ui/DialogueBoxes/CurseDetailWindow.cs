using TMPro;
using UnityEngine;

internal class CurseDetailWindow : MonoBehaviour
{
    public TextMeshProUGUI curse;
    public TextMeshProUGUI prophesy;
    public Transform imgParent;
    public Transform modsParent;

    public GameObject modifierPref;

    public void Init(CurseCard curseCard)
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

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
