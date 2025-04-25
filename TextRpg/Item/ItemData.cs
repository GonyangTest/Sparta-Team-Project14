using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    [Serializable]
    internal class ItemData
    {
        public string Type { get; set; } // "Weapon", "Armor", "Consumable"
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }

        // 무기 전용
        public int? Attack { get; set; }

        // 방어구 전용
        public int? Defense { get; set; }

        // 소비 아이템 전용
        public string OptionType { get; set; }
        public int? EffectValue { get; set; }
        public int? Amount { get; set; }

        public ItemData() { }

        // Item 객체에서 데이터 추출
        public static ItemData FromItem(Item item)
        {
            var itemData = new ItemData
            {
                Name = item.ItemName,
                Description = item.Description,
                Price = item.Price,
                IsPurchased = item.IsPurchased
            };

            // 아이템 타입에 따라 추가 정보 저장
            if (item is Weapon weapon)
            {
                itemData.Type = "Weapon";
                itemData.Attack = weapon.Attack;
            }
            else if (item is Armor armor)
            {
                itemData.Type = "Armor";
                itemData.Defense = armor.Defense;
            }
            else if (item is ConsumableItem consumable)
            {
                itemData.Type = "Consumable";
                itemData.OptionType = consumable.Option.ToString();  // consumable.Option 사용
                itemData.EffectValue = consumable.RecoveryAmount;   // consumable.RecoveryAmount 사용
                itemData.Amount = consumable.Quantity;
            }

            return itemData;
        }
        public Item ToItem()
        {
            switch (Type)
            {
                case "Weapon":
                    return new Weapon(Name, Attack.Value, Description, Price, IsPurchased);
                case "Armor":
                    return new Armor(Name, Defense.Value, Description, Price, IsPurchased);
                case "Consumable":
                    ConsumableItem.OptionType option =
                        (ConsumableItem.OptionType)Enum.Parse(typeof(ConsumableItem.OptionType), OptionType);
                    return new ConsumableItem(Name, Description, Price, option, EffectValue.Value, Amount.Value);
                default:
                    return new Item(Name, Description, Price, IsPurchased);
            }
        }
    }
}
