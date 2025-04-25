using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public static class ItemFactory
    {
        // 아이템 데이터베이스
        private static Dictionary<string, Item> _itemDatabase = new Dictionary<string, Item>();
        
        // 초기화 메서드
        static ItemFactory()
        {
            InitializeItems();
        }
        
        // 아이템 초기화
        private static void InitializeItems()
        {
            // 방어구 등록
            RegisterArmor("수련자 갑옷", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            RegisterArmor("무쇠갑옷", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500);
            RegisterArmor("스파르타의 갑옷", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            
            // 무기 등록
            RegisterWeapon("낡은 검", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            RegisterWeapon("청동 도끼", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            RegisterWeapon("스파르타의 창", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 4000);
            
            // 소비 아이템 등록
            RegisterConsumableItem(GameConstance.Item.HEALTH_POTION_NAME, 
                                  "체력을 회복하는 포션입니다.", 
                                  GameConstance.Item.HEALTH_POTION_PRICE, 
                                  ConsumableItem.OptionType.Health, 
                                  GameConstance.Item.HEALTH_POTION_HEAL_AMOUNT);
            
            RegisterConsumableItem(GameConstance.Item.MANA_POTION_NAME, 
                                  "마나를 회복하는 포션입니다.", 
                                  GameConstance.Item.MANA_POTION_PRICE, 
                                  ConsumableItem.OptionType.Mana, 
                                  GameConstance.Item.MANA_POTION_HEAL_AMOUNT);
        }
        
        // 무기 등록 메서드
        private static void RegisterWeapon(string name, int attack, string description, int price)
        {
            _itemDatabase[name] = new Weapon(name, attack, description, price);
        }
        
        // 방어구 등록 메서드
        private static void RegisterArmor(string name, int defense, string description, int price)
        {
            _itemDatabase[name] = new Armor(name, defense, description, price);
        }
        
        // 소비 아이템 등록 메서드
        private static void RegisterConsumableItem(string name, string description, int price, ConsumableItem.OptionType optionType, int recoveryAmount, int quantity = 1)
        {
            _itemDatabase[name] = new ConsumableItem(name, description, price, optionType, recoveryAmount, quantity);
        }
        // 모든 아이템 가져오기
        public static List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>();
            
            foreach (var item in _itemDatabase.Values)
            {
                // 원본이 수정되지 않도록 복사본 반환
                if (item is Weapon weapon)
                {
                    items.Add(new Weapon(item.itemName, weapon.attack, item.description, item.price, item.isPurchased));
                }
                else if (item is Armor armor)
                {
                    items.Add(new Armor(item.itemName, armor.defense, item.description, item.price, item.isPurchased));
                }
                else if (item is ConsumableItem consumable)
                {
                    items.Add(new ConsumableItem(item.itemName, item.description, item.price, consumable.Option, consumable.RecoveryAmount, consumable.Quantity));
                }
                else
                {
                    items.Add(new Item(item.itemName, item.description, item.price, item.isPurchased));
                }
            }
            
            return items;
        }
        
        // 이름으로 아이템 가져오기
        public static Item? GetItem(string name)
        {
            if (_itemDatabase.TryGetValue(name, out Item item))
            {
                // 원본이 수정되지 않도록 복사본 반환
                if (item is Weapon weapon)
                {
                    return new Weapon(item.itemName, weapon.attack, item.description, item.price, item.isPurchased);
                }
                else if (item is Armor armor)
                {
                    return new Armor(item.itemName, armor.defense, item.description, item.price, item.isPurchased);
                }
                else if (item is ConsumableItem consumable)
                {
                    return new ConsumableItem(item.itemName, item.description, item.price, consumable.Option, consumable.RecoveryAmount, consumable.Quantity);
                }

            }
            return null;
        }

        public static ConsumableItem? GetConsumableItem(string name, int quantity)
        {
            if (_itemDatabase.TryGetValue(name, out Item item))
            {
                if (item is ConsumableItem consumable)
                {
                    return new ConsumableItem(item.itemName, item.description, item.price, consumable.Option, consumable.RecoveryAmount, quantity);
                }
            }
            return null;
        }   
        
        // 무기 생성 메서드
        // public static Weapon? CreateWeapon(string name, int attack, string description, int price, bool isPurchased = false)
        // {
        //     return new Weapon(name, attack, description, price, isPurchased);
        // }
        
        // 방어구 생성 메서드
        // public static Armor? CreateArmor(string name, int defense, string description, int price, bool isPurchased = false)
        // {
        //     return new Armor(name, defense, description, price, isPurchased);
        // }

        public static Item CreateItem(string name)
        {
            return GetItem(name);
        }
        
        // 소비 아이템 생성 메서드
        public static ConsumableItem CreateConsumableItem(string name, int quantity = 1)
        {
            return GetConsumableItem(name, quantity);
        }
    }
} 