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
        private const int MaxMP = 100;

        public void DisplayRestMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("회복");
                Console.WriteLine($"[포션 아이템]");
                Console.WriteLine($"체력 포션 | 회복량 +{player.HealthPotion.RecoveryAmount} | 수량 : {player.HealthPotion.Quantity}");
                Console.WriteLine($"마나 포션 | 회복량 +{player.ManaPotion.RecoveryAmount} | 수량 : {player.ManaPotion.Quantity}");
                Console.WriteLine("\n1. 체력 회복하기");
                Console.WriteLine("2. 마나 회복하기");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                    return;

                if (input == "1")
                {
                    UsePotion(player, 1);
                    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
                else if (input == "2")
                {
                    UsePotion(player, 2);
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

        private void UsePotion(Player player, int index)
        {
            switch (index)
            {
                case 1:
                    player.UseHealthPotion();
                    break;
                case 2:
                    player.UseManaPotion();
                    break;
            }
        }
    }
}

