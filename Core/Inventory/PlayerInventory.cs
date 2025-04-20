using Newtonsoft.Json;
using SpartaDungeon.Core.Item;

namespace SpartaDungeon.Core.Inventory
{
    // 인벤토리 배열로 구현
    [Serializable]
    public class PlayerInventory : Interface.IPlayerInventory
    {
        [JsonProperty]
        private BaseItem[] _inventory;
        [JsonProperty]
        private int _inventorySize;
        [JsonProperty]
        private const int kmaxInventorySize = 30;

        public PlayerInventory()
        {
            _inventory = new BaseItem[kmaxInventorySize];
            _inventorySize = 0;
        }
        public void AddItem(BaseItem item)
        {
            if (_inventorySize >= kmaxInventorySize)
            {
                Console.WriteLine("인벤토리가 가득 찼습니다.");
                return;
            }
            _inventory[_inventorySize] = item;
            _inventorySize++;
        }

        public BaseItem? RemoveItem(int index)
        {
            if (index < 0 || index >= _inventorySize)
            {
                Console.WriteLine("잘못된 인덱스입니다.");
                return null;
            }

            BaseItem removedItem = _inventory[index];
            _inventory[index] = null;
            for (int i = index; i < _inventorySize - 1; i++)
            {
                _inventory[i] = _inventory[i + 1];
            }
            _inventorySize--;

            return removedItem;
        }

        public BaseItem? GetItemByIndex(int index)
        {
            if (index < 0 || index >= _inventorySize)
            {
                return null;
            }
            return _inventory[index];
        }

        public List<BaseItem> GetItemList()
        {
            List<BaseItem> itemList = new List<BaseItem>();
            for (int i = 0; i < _inventorySize; i++)
            {
                BaseItem item = _inventory[i];
                if (item != null)
                {
                    itemList.Add(item);
                }
            }
            return itemList;
        }
    }
}
