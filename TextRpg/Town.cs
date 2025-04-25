using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;


namespace TextRpg
{
    class Town
    {
        private const string TITLE = @"
            ███████╗██████╗  █████╗ ██████╗ ████████╗ █████╗         ██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ███████╗ ██████╗ ███╗   ██╗
            ██╔════╝██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝██╔══██╗        ██╔══██╗██║   ██║████╗  ██║██╔════╝ ██╔════╝██╔═══██╗████╗  ██║
            ███████╗██████╔╝███████║██████╔╝   ██║   ███████║        ██║  ██║██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██║   ██║██╔██╗ ██║
            ╚════██║██╔═══╝ ██╔══██║██╔══██╗   ██║   ██╔══██║        ██║  ██║██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║   ██║██║╚██╗██║
            ███████║██║     ██║  ██║██║  ██║   ██║   ██║  ██║        ██████╔╝╚██████╔╝██║ ╚████║╚██████╔╝███████╗╚██████╔╝██║ ╚████║
            ╚══════╝╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝        ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝
        
                            

                                        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣒⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣪⠀⡀⣀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⠀⠀⠀
                                        ⠐⠲⠇⠷⠸⠽⠨⠷⠪⠗⣅⠖⡿⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⢽⠽⣟⠯⠆⠀
                                        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢵⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠀⠀⠀
                                        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠣⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
        ";

        public void TownMap(Player player, Inventory inventory, Shop shop, Dungeon dungeon, Rest rest, Quest quest)
        {
            bool isExit = true;
            while (isExit)
            {
                Console.Clear();
                AnsiConsole.MarkupLine(TITLE);
                AnsiConsole.MarkupLine("[green]스파르타 마을[/]에 오신 여러분 환영합니다.");
                AnsiConsole.MarkupLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");    

                List<string> menu = new List<string>
                {
                    "1. 상태 보기",
                    "2. 인벤토리",
                    "3. 상점",
                    "4. 던전입장",
                    "5. 휴식하기",
                    "6. 퀘스트",
                    "7. 저장하기",
                    "0. 게임종료"
                };

				var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("원하시는 [green]행동[/]을 선택해주세요.")
                    .PageSize(10)
                    .AddChoices(menu));

                int index = int.Parse(choice.Split('.')[0]);


                switch (index)
                {
                    case 0:
                        Console.Clear();
                        isExit = false;
                        AnsiConsole.MarkupLine("게임종료");
                        break;
                    case 1:
                        Console.Clear();
                        AnsiConsole.MarkupLine(Markup.Escape(player.PrintPlayer()));
                        AnsiConsole.MarkupLine("\n0. 나가기");
                        Console.ReadLine();
                        break;
                    case 2:
                        inventory.ShowInventory(player);
                        break;
                    case 3:
                        shop.DisplayItems(inventory);
                        break;
                    case 4:
                        dungeon.EnterDungeonMenu(player);
                        break;
                    case 5:
                        rest.DisplayRestMenu(player);
                        break;
                    case 6:
                        Program.quest.PrintQuestList();
                        break;
                    case 7:
                        SaveLoadManager.SaveGame(player, inventory, quest, shop);
                        Console.WriteLine("계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
