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
        public int power = 0; // 공격력
        public int defense = 0; // 방어력
        public int basePower = 0;
        public int baseDefense = 0;
        public Item EquippedWeapon;
        public Item EquippedArmor;

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
                Console.WriteLine("1.전사\n2.도적\n");
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
                            hp = 125;
                            basePower = 5;
                            baseDefense = 5;
                            break;
                        case 2:
                            playerClass = "도적";
                            hp = 100;
                            basePower = 4;
                            baseDefense = 3;
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
        public int totalPower
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
                $"경험치: {exp}\n" +
                $"체력: {hp}\n" +
                $"공격력: {totalPower}\n" +
                $"방어력: {totalDefense}\n" +
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
