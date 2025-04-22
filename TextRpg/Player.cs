using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Job
    {
        public string Name;
        public int Hp;
        public int Mana;
        public int MaxHp;
        public int MaxMp;
        public float Power;
        public int Defense;
        public int Agility;

        public Job(string name, int hp, int mana, int maxHp, int maxMp, float power, int defense, int agility)
        {
            Name = name;
            Hp = hp;
            Mana = mana;
            MaxHp = maxHp;
            MaxMp = maxMp;
            Power = power;
            Defense = defense;
            Agility = agility;
        }

        public static Dictionary<int, Job> JobList = new Dictionary<int, Job>()
        {
            {1, new Job("전사", 150, 80, 150, 80, 8f, 6, 0)},
            {2, new Job("도적", 100, 100, 100, 100, 4f, 5, 0)},
            {3, new Job("궁수", 100, 100, 100, 100, 6f, 5, 0)},
            {4, new Job("마법사", 80, 150, 80, 150, 7f, 4, 0)}
        };
    }

    class Player
    {
        public string playerName;
        public string playerClass;
        public int level = 1;
        public int exp = 0;
        public int maxExp = 10;
        public int gold = 10000;
        public int hp;
        public int mana;
        public int maxHp;
        public int maxMp;
        public float power;
        public int defense;
        public int agility;
        public Job SelectedJob;

        public ConsumableItem HealthPotion = new ConsumableItem("체력포션", "체력을 회복하는 포션", 50, ConsumableItem.OptionType.Health, 30, 3);
        public ConsumableItem ManaPotion = new ConsumableItem("마나포션", "마나를 회복하는 포션", 100, ConsumableItem.OptionType.Mana, 30, 3);

        public Item EquippedWeapon;
        public Item EquippedArmor;

        // 체력 포션 사용
        

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

                SelectedJob = Job.JobList[selectJob];
                playerClass = SelectedJob.Name;
                hp = SelectedJob.Hp;
                mana = SelectedJob.Mana;
                maxHp = SelectedJob.MaxHp;
                maxMp = SelectedJob.MaxMp;
                power = SelectedJob.Power;
                defense = SelectedJob.Defense;
                agility = SelectedJob.Agility;

                break;
                
            }
        }

        public bool UseHealthPotion()
        {
            if (SelectedJob.Hp >= SelectedJob.MaxHp)
            {
                Console.WriteLine("\n이미 최대체력입니다.");
                return false;
            }

            return HealthPotion.Use(this);
        }

        // 마나 포션 사용
        public bool UseManaPotion()
        {
            if (SelectedJob.Mana >= SelectedJob.MaxMp)
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
                power += 0.5f;
                defense += 1;
            }

        }
        public float totalPower
        {
            get
            {
                float weaponBonus = EquippedWeapon is Weapon w ? w.attack : 0;
                return power + weaponBonus;
            }
        }

        public int totalDefense
        {
            get
            {
                int armorBonus = EquippedArmor is Armor a ? a.defense : 0;
                return defense + armorBonus;
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
                $"골드 : {gold}G\n";
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
