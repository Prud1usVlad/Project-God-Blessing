using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.Models;

public class PlayerStatsWindow : MonoBehaviour
{
    public StatsContainer playerStats;

    public GameObject statTextPrefab;
    public GameObject modifierPrefab;

    public Transform contentParent;

    private void Start()
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

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
