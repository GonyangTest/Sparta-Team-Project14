using Spectre.Console;
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
                Console.WriteLine($"마나 포션 | 회복량 +{player.ManaPotion.RecoveryAmount} | 수량 : {player.ManaPotion.Quantity}\n");

                string[] menuArray = new string[] { "1. 체력 회복하기", "2. 마나 회복하기", "0. 나가기" };

                var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
               .Title("무엇을 하시겠습니까?")
               .PageSize(3) // 항목 수
               .AddChoices(menuArray)
               .WrapAround()); // 리스트 순환 >> 맨 위 항목에서 위 방향키를 누르면 제일 아래 항목으로. 역도 성립

                // 선택한 메뉴가 몇번째인가?
                int index = 0;
                foreach (var menuTmp in menuArray)
                {
                    if (menu == menuTmp)
                        break;
                    else
                        index++;
                }
                if (index == 0)
                    UsePotion(player, 1);
                else if (index == 1)
                    UsePotion(player, 2);
                else
                    break;
                
                //Console.WriteLine("\n1. 체력 회복하기");
                //Console.WriteLine("2. 마나 회복하기");
                //Console.WriteLine("0. 나가기");
                //Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                //string input = Console.ReadLine();

                //if (input == "0")
                //    return;

                //if (input == "1")
                //{
                //    UsePotion(player, 1);
                //    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                //    Console.ReadKey();
                //}
                //else if (input == "2")
                //{
                //    UsePotion(player, 2);
                //    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                //    Console.ReadKey();
                //}
                //else
                //{
                //    Console.WriteLine("잘못된 입력입니다.");
                //    Console.ReadKey();
                //}
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
            Thread.Sleep(300); // 포션 사용/사용불가 메세지가 출력 후 보여지는 시간이 필요하여 추가
        }
    }
}

