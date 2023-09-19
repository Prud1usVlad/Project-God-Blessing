using Assets.Scripts.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HubSandboxUi : MonoBehaviour
{
    public Transform panel;
    public StatsContainer stats;
    public TextMeshProUGUI text;


    private void Awake()
    {
        foreach(var i in stats.Stats)
        {
            var o = Instantiate(text, panel);

            o.SetText($"{Enum.GetName(typeof(StatName), i.name)}: {i.Value}");
        }
    }

}
