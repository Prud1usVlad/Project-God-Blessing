using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buildings/Market", fileName = "Building")]
public class MarketBuilding : Building
{
    public int daysTillUpdate = 0;

    public NationName nation;
    public int daysToUpdate;
    public Dictionary<SlothType, EquipmentItem> currentStore;
    public IEnumerable<EquipmentItem> items => currentStore.Values;

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
                .Where(i => i.nation == nation && i.level == nationConnLevel)
                .ToList();
            
            var rand = UnityEngine.Random.Range(0, items.Count());
            var randItem = items.Count > 0 ? items[rand] : null;

            currentStore.Add(sloth, randItem);
        }
    }

    public void InitStore(IEnumerable<string> guids, int daysTillUpdate)
    {
        this.daysTillUpdate = daysTillUpdate;
        currentStore = new();

        foreach (var guid in guids)
        {
            var item = itemRegistry.FindByGuid(guid);
            currentStore.Add(item.slothType, item);
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

    public bool CanAffordItem(EquipmentItem item)
    {
        return gameProgress.resourceContainer.CanAfford(item.buyPrice);
    }

    public void BuyItem(EquipmentItem item)
    {
        if (!CanAffordItem(item))
            return;

        if (currentStore.Values.Contains(item))
        {
            gameProgress.resourceContainer.Spend(item.buyPrice);
            gameProgress.inventory.Add(item);
            currentStore.Remove(item.slothType);
        }
    }
}
