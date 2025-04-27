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
        public void DisplayRestMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                PrintPlayer(true);
                AnsiConsole.Write(new Rule("[red]사용[/][green]할 아이템을 선택하세요[/]"));
                Console.WriteLine("회복");
                Console.WriteLine($"[포션 아이템]");
                Console.WriteLine($"체력 포션 | 회복량 +{player.HealthPotion.RecoveryAmount} | 수량 : {player.HealthPotion.Quantity}");
                Console.WriteLine($"마나 포션 | 회복량 +{player.ManaPotion.RecoveryAmount} | 수량 : {player.ManaPotion.Quantity}\n");

                string[] menuArray = new string[] { "1. 체력 회복하기", "2. 마나 회복하기", "0. 나가기" };

                var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
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
                    UsePotion(player, 1); // 체력 포션 사용
                else if (index == 1)
                    UsePotion(player, 2); // 마나 포션 사용
                else
                    break;
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
            Console.SetCursorPosition(0,0);
            PrintPlayer(true);
            Thread.Sleep(GameConstance.Rest.USE_POTION_SLEEP_TIME); // 포션 사용/사용불가 메세지가 출력 후 보여지는 시간이 필요하여 추가
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true); // 버퍼에 남아있는 키를 읽고 버립니다.
            }
        }

        void PrintProgressBar(string label, double current, double max, Color completedColor, Color remainingColor)
        {
            int barWidth = 20; // 진행바 길이
            

            string colorCode;
            if (completedColor == Color.Red) colorCode = "red";
            else if (completedColor == Color.Blue) colorCode = "blue";
            else colorCode = "#6cf540";
            AnsiConsole
                .Progress()
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn
                    {
                        Width = barWidth,
                        CompletedStyle = new Style(foreground: completedColor),
                        RemainingStyle = new Style(foreground: remainingColor),
                        FinishedStyle = new Style(foreground: completedColor),
                    })
                .Start(x =>
                {
                    var task = x.AddTask($"[{colorCode}]{label}[/]");
                    float percentage = (float)(current * 100 / max);
                    task.Increment(percentage);
                });

            Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
            AnsiConsole.MarkupLine($"[{colorCode}]{current}/{max}[/]" + new string(' ', max.ToString().Length));
        }
        void PrintPlayer(bool isWithExpBar)
        {
            int partician_Length = 35; // 유닛 간 나누는 줄 길이

            AnsiConsole.MarkupLine($"[#6cf540]{new string('-', partician_Length)}[/]");
            // 플레이어
            AnsiConsole.MarkupLine($"[yellow bold]{Program.player.playerName}[/] ({Program.player.playerClass}) Lv. {Program.player.level}");
            //// HP바
            PrintProgressBar("HP ", Program.player.hp, Program.player.maxHp, Color.Red, Color.Red3);
            // MP바
            PrintProgressBar("MP ", Program.player.mana, Program.player.maxMp, Color.Blue, Color.Blue3);
            // Exp 바도 함께 출현
            if (isWithExpBar)
            {
                // 경험치 바 (초록)
                PrintProgressBar("EXP", Program.player.exp, Program.player.maxExp, Color.FromHex("#6cf540"), Color.FromHex("#62911A"));
            }
            // 아래 구분선
            AnsiConsole.MarkupLine($"[#6cf540]{new string('-', partician_Length)}[/]\n");
        }
    }
}

