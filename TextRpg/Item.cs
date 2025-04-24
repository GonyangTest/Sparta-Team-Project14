using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TextRpg
{
    class Item
    {
        // 상속개념 파악하기
        // 아이템 생성자 생성했으니까 출력하고 공격무기 방어무기 나눠 (상속)
        public string itemName;
        public string description;
        public int price;
        public bool isPurchased { get; set; }
        public Item(string name, string desc, int pri, bool isPurchased = false)
        {// 아이템에는 무기와 방어구가 있으므로 공통된 내용인 이름 설명 가격 구매여부를 인자값으로 생성자를 만든다.
            //초기화
            itemName = name;
            description = desc;
            price = pri;
            this.isPurchased = isPurchased;
        }
        public virtual void PrintInfo()
        {// 상점창에서 아이템을 나타내는 양식, '?'는 삼항 연산자를 사용한 조건문으로 true면 "구매완료", false면 가격을 표시
         // 삼항연산자 조건문(bool변수 ? true일때 : false일때)

            Console.WriteLine($" {itemName} | {description} | {(isPurchased ? "구매완료" : $"{price} G")}");
        }

    }
    class ConsumableItem : Item
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

        public OptionType Option { get => _optionType; set => _optionType = value; }
        public int RecoveryAmount { get => _recoveryAmount; set => _recoveryAmount = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }

        public ConsumableItem(string name, string desc, int price, OptionType option, int recoveryAmount, int quantity = 1)
            : base(name, desc, price, false)
        {
            _optionType = option;
            _recoveryAmount = recoveryAmount;
            _quantity = quantity;
        }

        // PrintInfo 메소드 오버라이드
        public override void PrintInfo()
        {
            Console.WriteLine($" {itemName} | {EnumUtils.GetDescription(_optionType)} +{_recoveryAmount} | {description} | {(isPurchased ? "구매완료" : $"{price} G")}");
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
            Quantity--;
            
            string effectType = EnumUtils.GetDescription(_optionType);
            
            Console.WriteLine($"{itemName}을(를) 사용했습니다. {effectType} +{_recoveryAmount} 효과를 얻었습니다.");
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
