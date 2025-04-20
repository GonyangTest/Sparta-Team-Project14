using SpartaDungeon.Core.Item;

namespace SpartaDungeon.Core.Inventory.Interface
{
    public interface IPlayerInventory
    {
        void AddItem(BaseItem item);
        BaseItem? RemoveItem(int index);
        BaseItem? GetItemByIndex(int index);
        List<BaseItem> GetItemList();
    }
} 