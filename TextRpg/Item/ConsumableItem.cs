using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TextRpg
{
    public class ConsumableItem : Item
    {
        public enum OptionType
        {
            [Description("체력 회복")]
            Health,     // 체력 회복
            [Description("마나 회복")]
            Mana,       // 마나 회복
        }
    
        private OptionType _optionType;
        private int _recoveryAmount;
        private int _quantity;

        public OptionType Option { get => _optionType; }
        public int RecoveryAmount { get => _recoveryAmount; }
        public int Quantity { get => _quantity; set => _quantity = value; }

        public ConsumableItem(string name, string desc, int price, OptionType option, int recoveryAmount, int quantity = 1)
            : base(name, desc, price, false)
        {
            _optionType = option;
            _recoveryAmount = recoveryAmount;
            _quantity = quantity;
        }

        // PrintInfo 메소드 오버라이드
        public override string GetInfo()
        {
            return $" {ItemName} | {EnumUtils.GetDescription(_optionType)} +{_recoveryAmount} | {Description} | {(_isPurchased ? "구매완료" : $"{_price} G")}";
        }
        // 아이템 사용
        public bool Use(Player player)
        {
            if (Quantity <= 0)
            {
                Console.WriteLine($"포션이 부족합니다.");
                return false;
            }

            // 효과 적용 전에 수량 감소
            _quantity--;
            
            string effectType = EnumUtils.GetDescription(_optionType);
            
            Console.WriteLine($"{ItemName}을(를) 사용했습니다. {effectType} +{_recoveryAmount} 효과를 얻었습니다.");
            Console.WriteLine($"남은 수량: {Quantity}");
            
            // 효과 적용
            ApplyEffect(player);
            
            return true;
        }

        // 효과 적용
        private void ApplyEffect(Player player)
        {
            switch (_optionType)
            {
                case OptionType.Health:
                    player.hp += _recoveryAmount;
                    if (player.hp >= player.maxHp)
                    {
                        player.hp = player.maxHp;
                    }
                    Console.WriteLine($"체력이 {_recoveryAmount} 회복되었습니다.");
                    break;
                case OptionType.Mana:
                    player.mana += _recoveryAmount;
                    if (player.mana >= player.maxMp)
                    {
                        player.mana = player.maxMp;
                    }
                    Console.WriteLine($"마나가 {_recoveryAmount} 회복되었습니다.");
                    break;
            }
        }
    }
}
