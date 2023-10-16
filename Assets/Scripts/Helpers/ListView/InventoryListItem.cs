using Assets.Scripts.EquipmentSystem;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.Helpers.ListView
{
    public class InventoryListItem : MonoBehaviour, IListItem
    {
        public InventoryRecord record;
        public EquipmentItem equipmentItem;

        public EquipmentItemRegistry equipmentRegistry;

        public Image underlay;
        public Image shade;
        public Image image;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            record = data as InventoryRecord;
            equipmentItem = equipmentRegistry
                .FindByGuid(record.itemGuid);

            if (record.isEquipped) 
            {
                shade.gameObject.SetActive(true);
                underlay.color = new Color(1, 0, 0, 0.5f);
                shade.color = new Color(0, 0, 0, 0.5f);
            }
        }

        public bool HasData(object data)
        {
            return record == data as InventoryRecord;
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
