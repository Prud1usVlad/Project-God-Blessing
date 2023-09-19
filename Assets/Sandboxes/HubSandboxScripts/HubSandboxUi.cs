using Assets.Scripts.EventSystem;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Event Raised");
            gameEvent.Raise();
        }
    }

    public void OnSomeEvent()
    {
        Debug.Log("Event responded");
    }

}
