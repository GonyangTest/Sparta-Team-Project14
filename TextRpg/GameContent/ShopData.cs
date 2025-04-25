using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    [Serializable]
    internal class ShopData
    {
        // 상점 아이템 목록 (이미 구현된 ItemData 클래스 활용)
        public List<ItemData> Items { get; set; }

        public ShopData()
        {
            Items = new List<ItemData>();
        }

        // Shop 객체로부터 아이템 목록과 구매 상태 저장
        public void FromShop(Shop shop)
        {
            Items.Clear();

            // Shop 클래스의 items 필드 접근
            var shopItems = shop.GetItems();

            foreach (var item in shopItems)
            {
                if (item != null)
                {
                    // 이미 구현된 ItemData.FromItem 메소드 활용
                    Items.Add(ItemData.FromItem(item));
                }
            }
        }

        // ShopData를 바탕으로 Shop 객체 업데이트 (구매 상태 복원)
        public void ToShop(Shop shop)
        {
            var shopItems = shop.GetItems();

            // 아이템 이름으로 매칭하여 구매 상태 복원
            foreach (var shopItem in shopItems)
            {
                if (shopItem != null)
                {
                    // 저장된 데이터에서 일치하는 아이템 찾기
                    var savedItem = Items.FirstOrDefault(i => i.Name == shopItem.itemName);
                    if (savedItem != null)
                    {
                        // 구매 상태 복원
                        shopItem.isPurchased = savedItem.IsPurchased;
                    }
                }
            }
        }

    }
}
