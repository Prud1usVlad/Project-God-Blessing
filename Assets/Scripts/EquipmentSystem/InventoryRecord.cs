using System;
using UnityEngine;

namespace Assets.Scripts.EquipmentSystem
{
    // used for saving and collecting diferrent
    // instances of items referencing the same item data 
    [Serializable]
    public class InventoryRecord
    {
        private EquipmentItem _item;

        public string itemGuid;
        public string recordGuid;
        public bool isEquipped;
        public int level;

        public EquipmentItem item => _item;

        public InventoryRecord() { }
        public InventoryRecord(EquipmentItem item, int level) 
        {
            itemGuid = item.Guid;
            this._item = item;
            recordGuid = Guid.NewGuid().ToString();
            this.level = level;
        }

        public void ChangeItem(EquipmentItem equipmentItem)
        {
            _item = equipmentItem;
            itemGuid = equipmentItem.Guid;
        }
    }
}
