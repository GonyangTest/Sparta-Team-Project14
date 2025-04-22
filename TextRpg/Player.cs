using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{

    class Player
    {
        public string playerName = ""; // 이름
        public string playerClass = ""; // 직업정보
        public int level = 1; // 레벨
        public int exp = 0; // 경험치
        public int gold = 10000; // 골드
        public int hp = 0; // 체력
        public float power = 0.0f; // 공격력
        public int defense = 0; // 방어력
        public float basePower = 0.0f;
        public int baseDefense = 0;
        //추가스텟
        public int mana = 0;
        public int agility = 0; // 민첩
        public int maxHp = 100; // 최대 체력
        public int maxMp = 100; // 최대 마나
        public int maxExp = 10; // 최대 경험치

        public ConsumableItem HealthPotion = new ConsumableItem("체력포션", "체력을 회복하는 포션", 50, ConsumableItem.OptionType.Health, 30, 3);
        public ConsumableItem ManaPotion = new ConsumableItem("마나포션", "마나를 회복하는 포션", 100, ConsumableItem.OptionType.Mana, 30, 3);

        public Item EquippedWeapon;
        public Item EquippedArmor;

        // 체력 포션 사용
        public bool UseHealthPotion()
        {
            if (hp >= maxHp)
            {
                Console.WriteLine("\n이미 최대체력입니다.");
                return false;
            }

            return HealthPotion.Use(this);
        }

        // 마나 포션 사용
        public bool UseManaPotion()
        {
            if (mana >= maxMp)
            {
                Console.WriteLine("\n이미 최대마나입니다.");
                return false;
            }

            return ManaPotion.Use(this);
        }

        // 레벨업시 변화
        public void LevelUp()
        {
            while (exp >= maxExp)
            {
                level++;
                exp -= maxExp;

                // 필요 경험치 증가량 로직 생성
                int increase = 25 + (level - 2) * 5;
                maxExp += increase;

                // 레벨업시 공격력 0.5증가 방어력 1증가
                basePower += 0.5f;
                baseDefense += 1;
            }

        }

        public void SetPlayer()
        {

            while (true)
            {
                bool isSave = true;
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 이름을 설정해주세요.");
                string userName = Console.ReadLine();
                playerName = userName;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"입력하신 이름은 {playerName}입니다.\n");
                    Console.WriteLine("1.저장\n2.취소\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    string userNameSelect = Console.ReadLine();
                    int userNameTmp;
                    if (!int.TryParse(userNameSelect, out userNameTmp))
                    {
                        Console.Clear();
                        Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                        Console.ReadKey();
                    }
                    else
                    {
                        switch (userNameTmp)
                        {
                            case 1:
                                playerName = userName;
                                break;
                            case 2:
                                Console.Clear();
                                isSave = false;
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                                Console.ReadKey();
                                continue;
                        }
                    }
                    break;
                }
                if (isSave)
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 직업을 설정해주세요.\n");
                Console.WriteLine("1.전사\n2.도적\n3.궁수\n4.마법사\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                string job = Console.ReadLine();
                int selectJob;
                if (!int.TryParse(job, out selectJob))
                {
                    Console.Clear();
                    Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                    Console.ReadKey();
                }
                else
                {
                    switch (selectJob)
                    {
                        case 1:
                            playerClass = "전사";
                            hp = 150;
                            maxHp = 150;
                            basePower = 8f;
                            baseDefense = 6;
                            agility = 0;
                            mana = 80;
                            maxMp = 80;
                            break;
                        case 2:
                            playerClass = "도적";
                            hp = 100;
                            maxHp = 100;
                            basePower = 4f;
                            baseDefense = 5;
                            agility = 20;
                            mana = 100;
                            maxMp = 100;
                            break;
                        case 3:
                            playerClass = "궁수";
                            hp = 100;
                            maxHp = 100;
                            basePower = 6f;
                            baseDefense = 5;
                            agility = 10;
                            mana = 100;
                            maxMp = 100;
                            break;
                        case 4:
                            playerClass = "마법사";
                            hp = 80;
                            maxHp = 85;
                            basePower = 7f;
                            baseDefense = 4;
                            agility = 0;
                            mana = 150;
                            maxMp = 150;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                            Console.ReadKey();
                            continue;
                    }
                    break;
                }
            }
        }
        public float totalPower
        {
            get
            {
                int weaponBonus = EquippedWeapon is Weapon w ? w.attack : 0;
                return basePower + weaponBonus;
            }
        }

        public int totalDefense
        {
            get
            {
                int armorBonus = EquippedArmor is Armor a ? a.defense : 0;
                return baseDefense + armorBonus;
            }
        }
        public string CurrentPlayer()
        {
            while (true)
            {
                Console.WriteLine("상태보기\n");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                return $"[플레이어 정보]\n" +
                $"이름: {playerName}\n" +
                $"직업: {playerClass}\n" +
                $"레벨: {level}\n" +
                $"경험치: {exp}/{maxExp}\n" +
                $"체력: {hp}/{maxHp}\n" +
                $"마력: {mana}/{maxMp}\n" +
                $"공격력: {totalPower}\n" +
                $"방어력: {totalDefense}\n" +
                $"민첩: {agility}\n" +
                $"장착 무기: {(EquippedWeapon != null ? EquippedWeapon.itemName : "없음")}\n" +
                $"장착 방어구: {(EquippedArmor != null ? EquippedArmor.itemName : "없음")}\n" +
                $"골드 : {gold}\n";
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                string current = Console.ReadLine();
                int currentSelect;
                if (!int.TryParse(current, out currentSelect))
                {
                    Console.Clear();
                    Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                    Console.ReadKey();
                }
                else
                {
                    switch (currentSelect)
                    {
                        case 0:
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                            Console.ReadKey();
                            continue;
                    }
                }
                break;
            }
        }
    }
}
