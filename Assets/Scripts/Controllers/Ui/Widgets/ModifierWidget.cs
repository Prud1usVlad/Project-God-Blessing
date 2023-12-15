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
    public TextMeshProUGUI source;

    public Color plusColor;
    public Color minusColor;

    public void UpdateView(StatMod statModifier, bool showSource = false)
    {
        var value = statModifier.modifier.Value;
        statName.SetText(Enum.GetName(typeof(StatName), statModifier.stat));

        SetValue(value, statModifier.modifier.Type);
        SetSource(showSource, statModifier.modifier.Source);
    }

    public void UpdateView(ResMod resModifier, bool showSource = false)
    {
        var resName = Enum.GetName(typeof(ResourceName), resModifier.resource);
        var gain = (resModifier.forGain ? "Gain" : "Use");
        var trans = Enum.GetName(typeof(TransactionType), resModifier.modifier.Transaction);

        statName.SetText($"{gain} in {trans} of {resName}");

        SetValue(resModifier.modifier.Value, resModifier.modifier.Type);
        SetSource(showSource, resModifier.modifier.Source);
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

    private void SetSource(bool showSource, object source)
    {
        if (showSource && source is not null)
        {
            this.source.SetText("Source: " + (source as ScriptableObject).name);
            this.source.gameObject.SetActive(true);
        }
        else
            this.source.gameObject.SetActive(false);
    }
}
