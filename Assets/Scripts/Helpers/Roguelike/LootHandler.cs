using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EquipmentSystem;
using Unity.VisualScripting;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
    public List<ResourceData> Resources;

    public List<CollectedResourceData> CollectedResources
    {
        get;
        private set;
    }
    public List<EquipmentItem> CollectedItems
    {
        get;
        private set;
    }

    public EquipmentItemRegistry EquipmentItemRegistry;

    public static LootHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<LootHandler>();

                if (_instance == null)
                {
                    throw new Exception("Loot handler does not exist");
                }
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private static LootHandler _instance;

    private LootHandler() { }

    public void CollectResources(CollectedResourceData resourceData)
    {
        int index = CollectedResources.FindIndex(x => x.Name.Equals(resourceData.Name));

        if(index == -1)
        {
            CollectedResources.Add(resourceData);
        }
        else
        {
            CollectedResources[index].LootedAmount += resourceData.LootedAmount;
        }
    }

    public void CollectItem(EquipmentItem item)
    {
        CollectedItems.Add(item);
    }

    private void Start()
    {
        if (!EquipmentItemRegistry.GetAll().Any())
        {
            throw new Exception("Empty item register");
        }

        CollectedResources = new List<CollectedResourceData>();
        CollectedItems = new List<EquipmentItem>();
    }

    private void Update()
    {

    }
}
