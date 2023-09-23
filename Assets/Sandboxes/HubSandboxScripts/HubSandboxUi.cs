using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.Stats;
using System;
using TMPro;
using UnityEngine;

public class HubSandboxUi : MonoBehaviour
{
    public Transform panel;
    public StatsContainer stats;
    public TextMeshProUGUI text;

    public GameEvent gameEvent;

    private void Awake()
    {
        foreach(var i in stats.Stats)
        {
            var o = Instantiate(text, panel);

            o.SetText($"{Enum.GetName(typeof(StatName), i.name)}: {i.Value}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Increase");
            gameEvent.Raise("Increase");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Decrease");
            gameEvent.Raise("Decrease");
        }

        CleanChildren(panel);
        foreach (var i in stats.Stats)
        {
            var o = Instantiate(text, panel);

            o.SetText($"{Enum.GetName(typeof(StatName), i.name)}: {i.Value}");
        }
    }

    public void OnAdd(string parameter)
    {
        if (parameter == "Increase")
        {
            stats.GetStat(StatName.Sanity).AddModifier(
                new StatModifier(2, ModifierType.Flat, source: this));
        }

        if (parameter == "Decrease")
        {
            stats.GetStat(StatName.Sanity).AddModifier(
                new StatModifier(-2, ModifierType.Flat, source: this));
        }
    }

    public void CleanChildren(Transform obj)
    {
        int nbChildren = obj.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(obj.GetChild(i).gameObject);
        }
    }
}
