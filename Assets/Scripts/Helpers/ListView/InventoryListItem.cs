﻿using Assets.Scripts.EquipmentSystem;
using System;
using UnityEngine.UI;
using UnityEngine;
using JetBrains.Annotations;

namespace Assets.Scripts.Helpers.ListView
{
    public class InventoryListItem : MonoBehaviour, IListItem
    {
        public InventoryRecord record;
        public EquipmentItem equipmentItem;

        public Equipment equipment;
        public EquipmentItemRegistry equipmentRegistry;

        public Image underlay;
        public Image shade;
        public Image image;
        public InventoryItemTooltipTrigger tooltipTrigger;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            if (data == null)
            {
                shade.gameObject.SetActive(false);

                return;
            }
            else if (data is InventoryRecord)
            {
                record = data as InventoryRecord;
                equipmentItem = equipmentRegistry
                    .FindByGuid(record.itemGuid);
            }
            else if (data is EquipmentItem)
            {
                equipmentItem = data as EquipmentItem;
            }

            ProcessEquipmentItem();

        }

        private void ProcessEquipmentItem()
        {
            if (record.isEquipped)
            {
                shade.gameObject.SetActive(true);
                shade.color = new Color(0, 0, 0, 0.5f);
            }

            tooltipTrigger.Init(equipmentItem,
                equipment.GetEquipedAnalogue(equipmentItem));
        }

        public bool HasData(object data)
        {
            if (data is InventoryRecord)
                return record == data as InventoryRecord;
            else if (data is EquipmentItem)
                return equipmentItem == data as EquipmentItem;
            else return false;
        }

        public void OnSelected()
        {
            
        }

        public void OnSelecting()
        {
            Selection?.Invoke();
        }
    }
}
