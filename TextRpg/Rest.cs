using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    class Rest
    {
        private const int RestCost = 1200;
        private const int MaxHp = 100;

        public void DisplayRestMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("휴식하기");
                Console.WriteLine($"{RestCost} G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.gold} G)");
                Console.WriteLine("\n1. 휴식하기");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                    return;

                if (input == "1")
                {
                    RestArea(player);
                    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }

        private void RestArea(Player player)
        {
            if (player.hp >= MaxHp)
            {
                Console.WriteLine("\n이미 최대체력입니다.");
            }
            else if (player.gold >= RestCost)
            {
                player.gold -= RestCost;
                player.hp = MaxHp;
                Console.WriteLine("\n휴식을 완료했습니다. 체력이 모두 회복되었습니다!");
                Console.WriteLine($"[남은 골드] {player.gold} G");
                Console.WriteLine($"[현재 체력] {player.hp} / {MaxHp}");
            }
            else
            {
                Console.WriteLine("\nGold가 부족합니다.");
            }
            
        }
    }
}

