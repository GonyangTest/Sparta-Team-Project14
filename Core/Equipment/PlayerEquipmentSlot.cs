using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json.Serialization;
using SpartaDungeon.Core.Equipment.Interface;
using SpartaDungeon.Core.Item;
using Newtonsoft.Json;

namespace SpartaDungeon.Core.Equipment
{
    // 인벤토리 배열로 구현
    [Serializable]
    public class PlayerEquipmentSlot : IPlayerEquipmentSlot
    {
        [JsonProperty]
        private Dictionary<ItemType, BaseItem?> _equipmentSlots;

        public PlayerEquipmentSlot()
        {
            _equipmentSlots = new Dictionary<ItemType, BaseItem?>();
        }

        public BaseItem? EquipItem(ItemType slotType, BaseItem item)
        {
            if(item == null)
            {
                Console.WriteLine("잘못된 아이템입니다.");
                return null;
            }

            BaseItem? previousItem = _equipmentSlots.ContainsKey(slotType) ? _equipmentSlots[slotType] : null;
            _equipmentSlots[slotType] = item;
            return previousItem;
        }

        public BaseItem? UnEquipItem(ItemType slotType)
        {
            BaseItem? unEquippedItem = _equipmentSlots[slotType];
            _equipmentSlots[slotType] = null;
            return unEquippedItem;
        }

        public BaseItem? RemoveItem(ItemType slotType)
        {
            if (_equipmentSlots.ContainsKey(slotType))
            {
                BaseItem? item = _equipmentSlots[slotType];
                _equipmentSlots[slotType] = null;
                return item;
            }
            return null;
        }

        // TODO: 변수정 수정
        public BaseItem? GetItemByIndex(ItemType slotType)
        {
            return _equipmentSlots[slotType];
        }

        public List<BaseItem> GetItemList()
        {
            List<BaseItem> items = new List<BaseItem>();
            foreach (var item in _equipmentSlots)
            {
                if (item.Value != null)
                {
                    items.Add(item.Value);
                }
            }
            return items;
        }
        public int GetItemCount()
        {
            int count = 0;
            foreach(var item in _equipmentSlots)
            {
                if(item.Value != null)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
