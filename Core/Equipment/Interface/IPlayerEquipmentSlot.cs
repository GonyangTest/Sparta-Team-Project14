using SpartaDungeon.Core.Item;

namespace SpartaDungeon.Core.Equipment.Interface
{
    public interface IPlayerEquipmentSlot
    {
        BaseItem? EquipItem(ItemType slotType, BaseItem item);
        BaseItem? UnEquipItem(ItemType slotType);
        BaseItem? RemoveItem(ItemType slotType);
        BaseItem? GetItemByIndex(ItemType slotType);
        List<BaseItem> GetItemList();
        int GetItemCount();
    }
} 