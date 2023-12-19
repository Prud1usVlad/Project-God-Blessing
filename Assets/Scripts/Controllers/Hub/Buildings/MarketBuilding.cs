using Assets.Scripts.EquipmentSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Market", fileName = "Building")]
public class MarketBuilding : Building
{
    public int daysTillUpdate = 0;

    public NationName nation;
    public int daysToUpdate;
    public Dictionary<SlothType, InventoryRecord> currentStore;
    public IEnumerable<InventoryRecord> items => currentStore?.Values;

    public EquipmentItemRegistry itemRegistry;
    public GameProgress gameProgress;

    public override void InitDialogue(DialogueBox dialogueBox, BuildingController controller = null)
    {
        (dialogueBox as MarketBuildingDialogueBox).building = this;
        modalManager.DialogueOpen(dialogueBox);
    }

    public void UpdateStore()
    {
        currentStore = new();

        foreach (var sloth in (SlothType[])Enum.GetValues(typeof(SlothType)))
        {
            var nationConnLevel = gameProgress
                .skillSystem.connections.GetLevel(nation);
            var items = itemRegistry
                .GetBySlothType(sloth)
                .Where(i => i.nation == nation)
                .ToList();
            
            if (items.Any()) 
            {
                var rand = UnityEngine.Random.Range(0, items.Count());
                var randItem = items.Count > 0 ? items[rand] : null;
                currentStore.Add(sloth, new InventoryRecord(randItem, nationConnLevel));
            }
            else
            {
                currentStore.Add(sloth, null); ;
            }

        }
    }

    public void InitStore(IEnumerable<string> guids, int daysTillUpdate, int itemsLevel)
    {
        this.daysTillUpdate = daysTillUpdate;
        currentStore = new();

        foreach (var guid in guids)
        {
            var item = itemRegistry.FindByGuid(guid);
            currentStore.Add(item.slothType, new InventoryRecord(item, itemsLevel));
        }

        foreach (var sloth in (SlothType[])Enum.GetValues(typeof(SlothType)))
        {
            if (!currentStore.ContainsKey(sloth))
                currentStore.Add(sloth, null);
        }
    }

    public void NextDay()
    {
        if (--daysTillUpdate == 0)
        {
            UpdateStore();
            daysTillUpdate = daysToUpdate;
        }
    }

    public bool CanAffordItem(InventoryRecord record)
    {
        return gameProgress.resourceContainer.CanAfford(record.item.buyPrice);
    }

    public void BuyItem(InventoryRecord record)
    {
        if (!CanAffordItem(record))
            return;

        if (currentStore.Values.Contains(record))
        {
            gameProgress.resourceContainer.Spend(record.item.buyPrice);
            gameProgress.inventory.Add(record);
            currentStore.Remove(record.item.slothType);
        }
    }
}
