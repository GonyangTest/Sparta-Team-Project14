using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    [Serializable]
    internal class InventoryData
    {
        public List<ItemData> Items { get; set; } = new List<ItemData>();

        public InventoryData() { }

        // Inventory 객체에서 데이터 추출
        public void FromInventory(Inventory inventory)
        {
            Items.Clear();
            foreach (var item in inventory.getInventory())
            {
                Items.Add(ItemData.FromItem(item));
            }
        }

        // InventoryData 객체를 Inventory 객체에 적용
        public void ToInventory(Inventory inventory)
        {
            inventory.getInventory().Clear();
            foreach (var itemData in Items)
            {
                inventory.getInventory().Add(itemData.ToItem());
            }
        }
    }
}
