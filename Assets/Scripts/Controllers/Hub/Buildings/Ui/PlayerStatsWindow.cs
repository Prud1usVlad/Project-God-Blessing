using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.Models;

public class PlayerStatsWindow : DialogueBox
{
    public StatsContainer playerStats;

    public GameObject statTextPrefab;
    public GameObject modifierPrefab;

    public Transform contentParent;

    private void UpdateView()
    {
        foreach (var stat in playerStats.Stats)
        {
            var tmp = Instantiate(statTextPrefab, contentParent)
                .GetComponent<TextMeshProUGUI>();

            tmp.SetText($"{Enum.GetName(typeof(StatName), stat.name)}: {stat.Value}");
            tmp.gameObject.SetActive(true);

            foreach (var mod in stat.modifiers)
            {
                Instantiate(modifierPrefab, contentParent)
                    .GetComponent<ModifierWidget>()
                    .UpdateView(new StatMod
                    {
                        modifier = mod,
                        reciever = ModifierReciever.Player,
                        stat = stat.name,
                    }, true);
            }
        }
    }

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        if (inited)
            UpdateView();

        return inited;
    }

    public void OnClose()
    {
        modalManager.DialogueClose();
    }
}
