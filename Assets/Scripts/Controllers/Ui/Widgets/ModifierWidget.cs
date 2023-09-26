using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.Stats;
using System;
using TMPro;
using UnityEngine;

public class ModifierWidget : MonoBehaviour
{
    public TextMeshProUGUI statName;
    public TextMeshProUGUI modValue;

    public Color plusColor;
    public Color minusColor;

    public void UpdateView(StatMod statModifier)
    {
        var value = statModifier.modifier.Value;
        statName.SetText(Enum.GetName(typeof(StatName), statModifier.stat));

        SetValue(value, statModifier.modifier.Type);


    }

    public void UpdateView(ResMod resModifier)
    {
        var resName = Enum.GetName(typeof(ResourceName), resModifier.resource);
        var gain = (resModifier.forGain ? "Gain" : "Use");
        var trans = Enum.GetName(typeof(TransactionType), resModifier.modifier.Transaction);

        statName.SetText($"{gain} in {trans} of {resName}");

        SetValue(resModifier.modifier.Value, resModifier.modifier.Type);
    }

    private void SetValue(float value, ModifierType type)
    {
        var valueText = $"{value}";

        if (value >= 0)
        {
            valueText = "+" + valueText;
            modValue.color = plusColor;
        }
        else
        {
            modValue.color = minusColor;
        }

        if (type == ModifierType.Flat) { valueText += "pt."; }
        else { valueText += "%"; }

        modValue.SetText(valueText);
    }
}
