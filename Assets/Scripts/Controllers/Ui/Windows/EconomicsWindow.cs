using Assets.Scripts.ResourceSystem;
using System;
using System.Linq;
using UnityEngine;

public class EconomicsWindow : MonoBehaviour
{
    public GameObject resourcePrefab;

    public ResourceContainer container;

    public ListViewController listView;

    private void Start()
    {
        listView.InitView(container.Resources.Cast<object>().ToList());
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            container.GainResource(ResourceName.Gold, 100, TransactionType.Loot);
            listView.RefreshList();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            container.SpendResource(ResourceName.Gold, 100, TransactionType.Sell);
            listView.RefreshList();
        }
    }

}
