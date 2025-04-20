using SpartaDungeon.Core.Item;

namespace SpartaDungeon.Core.Shop.Interface
{
    public interface IItemShop
    {
        void Init();
        BaseItem GetShopItem(int id);
        List<BaseItem> GetShopItemList();
        int GetItemIndex(BaseItem item);
        BaseItem? GetItem(int index);
        bool HasStock(int index);
        void DecreaseStock(int index);
        void IncreaseStock(int index);
        void IncreaseStock(BaseItem item);
    }
} 