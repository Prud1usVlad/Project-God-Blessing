using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Helpers.Roguelike;
using UnityEngine;

public class TreasureLootController : MonoBehaviour
{
    public List<TreasureData> TreasureData;

    private LootHandler _lootHandler;

    private void Start()
    {
        _lootHandler = LootHandler.Instance;
    }

    public void Loot()
    {
        InventoryRecord collectedItem = null;
        foreach (TreasureData data in TreasureData)
        {
            if (!RandomHelper.GetResultOfChanceWheel(data.LootChanse))
            {
                continue;
            }

            List<EquipmentItem> items = data.ItemType.Equals(ItemType.None) 
                ? _lootHandler.EquipmentItemsList
                : _lootHandler.EquipmentItemsList.Where(x => x.type.Equals(data.ItemType)).ToList();

            EquipmentItem item = items[Random.Range(0, items.Count)];

            collectedItem = new InventoryRecord(item, Random.Range(data.MinItemLevel, data.MaxItemLevel));

            break;
        }

        if (collectedItem == null)
        {
            TreasureData data = TreasureData[Random.Range(0, TreasureData.Count)];

            List<EquipmentItem> items = data.ItemType.Equals(ItemType.None) 
                ? _lootHandler.EquipmentItemsList
                : _lootHandler.EquipmentItemsList.Where(x => x.type.Equals(data.ItemType)).ToList();

            EquipmentItem item = items[Random.Range(0, items.Count)];

            collectedItem = new InventoryRecord(item, Random.Range(data.MinItemLevel, data.MaxItemLevel));
        }


        _lootHandler.CollectItem(collectedItem);
    }
}