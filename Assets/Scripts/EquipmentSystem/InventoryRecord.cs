using System;

namespace Assets.Scripts.EquipmentSystem
{
    // used for saving and collecting diferrent
    // instances of items referencing the same item data 
    [Serializable]
    public class InventoryRecord
    {
        public string itemGuid;
        public string recordGuid;
        public bool isEquipped;

        public InventoryRecord() { }
        public InventoryRecord(string itemGuid) 
        {
            this.itemGuid = itemGuid;
            recordGuid = Guid.NewGuid().ToString();
        }
        public InventoryRecord(EquipmentItem item) : this(item.Guid) { }
    }
}
