using SpartaDungeon.Core.Data;
using SpartaDungeon.Core.Item;
using SpartaDungeon.Core.Shop.Interface;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace SpartaDungeon.Core.Shop
{
    [Serializable]
    public class ItemShop : IItemShop
    {
        [JsonProperty]
        public Dictionary<int, BaseItem> _items = new Dictionary<int, BaseItem>();
        [JsonProperty]
        public Dictionary<int, int> _itemCount = new Dictionary<int, int>();

        public ItemShop()
        {

        }

        public BaseItem GetShopItem(int id)
        {
            if (_items.ContainsKey(id))
            {
                return _items[id];
            }
            else
            {
                return null;
            }
        }

        public void Init()
        {
            _items.Add(0, ItemDataBase.Instance.CreateItem(1));
            _items.Add(1, ItemDataBase.Instance.CreateItem(2));
            _items.Add(2, ItemDataBase.Instance.CreateItem(3));
            _items.Add(3, ItemDataBase.Instance.CreateItem(4));
            _items.Add(4, ItemDataBase.Instance.CreateItem(5));
            _items.Add(5, ItemDataBase.Instance.CreateItem(6));
            _items.Add(6, ItemDataBase.Instance.CreateItem(7));
            _items.Add(7, ItemDataBase.Instance.CreateItem(8));
            _items.Add(8, ItemDataBase.Instance.CreateItem(9));

            for (int i = 0; i < _items.Count; i++) 
            {
                _itemCount.Add(i, 1);
            }
        }

        public List<BaseItem> GetShopItemList()
        {
            List<BaseItem> itemList = new List<BaseItem>();
            foreach (var item in _items)
            {
                itemList.Add(item.Value);
            }
            return itemList;
        }
        public int GetItemIndex(BaseItem item)
        {
            foreach (var pair in _items)
            {
                if (pair.Value.Id == item.Id)
                {
                    return pair.Key;
                }
            }
            return -1;
        }

        public BaseItem? GetItem(int index)
        {
            if (_items.ContainsKey(index))
            {
                return _items[index];
            }
            else
            {
                return null;
            }
        }

        public bool HasStock(int index)
        {
            return _itemCount[index] > 0 ? true : false;
        }
        public void DecreaseStock(int index)
        {
            _itemCount[index]--;
        }
        public void IncreaseStock(int index)
        {
            _itemCount[index]++;
        }
        public void IncreaseStock(BaseItem item)
        {
            int index = GetItemIndex(item);
            _itemCount[index]++;
        }
    }
}