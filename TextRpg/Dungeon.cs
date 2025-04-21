using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    class Dungeon
    {
        Random rand = new Random();

        public void EnterDungeonMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("**던전입장**");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
                Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
                Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
                Console.WriteLine("0. 나가기\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        EnterDungeon(player, "쉬운 던전", 5, 1000);
                        break;
                    case "2":
                        EnterDungeon(player, "일반 던전", 11, 1700);
                        break;
                    case "3":
                        EnterDungeon(player, "어려운 던전", 17, 2500);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void EnterDungeon(Player player, string dungeonName, int requiredDefense, int baseReward)
        {
            Console.Clear();
            Console.WriteLine($"[{dungeonName}] 입장 중...\n");

            float def = player.totalDefense;
            float atk = player.totalPower;

            int originHp = player.hp;
            int originGold = player.gold;

            bool cleared = true;

            // 실패 판정
            if (def < requiredDefense)
            {
                int failChance = rand.Next(0, 100);
                if (failChance < 40)
                {
                    player.hp /= 2;
                    cleared = false;
                    Console.WriteLine("던전 실패!\n");
                    Console.WriteLine("[탐험 결과]");
                    Console.WriteLine($"체력 {originHp} -> {player.hp}");
                    Console.WriteLine("보상 없음");
                    Console.WriteLine("\n0. 나가기");
                    Console.ReadLine();
                    return;
                }
            }

            // 성공 처리
            int defGap = requiredDefense - (int)def;
            int hpLossMin = 20 + Math.Max(0, defGap);
            int hpLossMax = 35 + Math.Max(0, defGap);
            int hpLoss = rand.Next(hpLossMin, hpLossMax + 1);
            player.hp = Math.Max(0, player.hp - hpLoss);

            // 보상 계산
            int bonusPercent = rand.Next((int)atk, (int)atk * 2 + 1);
            int bonusGold = baseReward * bonusPercent / 100;
            int totalGold = baseReward + bonusGold;
            player.gold += totalGold;

            Console.WriteLine("**던전 클리어**");
            Console.WriteLine($"축하합니다!!\n{dungeonName}을 클리어 하였습니다.\n");
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {originHp} -> {player.hp}");
            Console.WriteLine($"Gold {originGold} G -> {player.gold} G");
            Console.WriteLine("\n0. 나가기");
            Console.ReadLine();
        }
    }
}
