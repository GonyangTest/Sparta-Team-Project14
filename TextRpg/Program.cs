using System.Runtime.InteropServices;
using TextRpg;
using System.Text;
using Spectre.Console;
using System.Numerics;

internal class Program
{

    internal static Player player = new Player();
    internal static Inventory inventory = new Inventory();
    internal static Quest quest = new Quest();
    internal static Shop shop;


    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.SetWindowSize(150, 60);

        // 게임 불러오기
        if (SaveLoadManager.SaveFileExists())  // 저장된 게임 파일이 있으면 불러오기
        {
            Console.WriteLine("저장된 기록이 있습니다. 불러오시겠습니까?");
            List<string> startMenu = new List<string>
                   {
                       "1. 불러오기",
                       "2. 새 시작",
                       "0. 게임종료"
                   };
            var startChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("원하시는 [green]행동[/]을 선택해주세요.")
                    .PageSize(10)
                    .AddChoices(startMenu)
                    .WrapAround(true)); 

            int index = int.Parse(startChoice.Split('.')[0]);
            switch (index)
            {
                case 0:
                    Console.WriteLine("게임이 종료되었습니다.");  // 종료 메시지 출력
                    Environment.Exit(0);
                    break;
                case 1:
                    shop ??= new Shop(player);
                    bool loaded = SaveLoadManager.LoadGame(player, inventory, quest, shop);
                    if (loaded)
                    {
                        Console.WriteLine("저장된 게임을 성공적으로 불러왔습니다.");
                    }
                    else
                    {
                        Console.WriteLine("게임 불러오기 실패.");
                    }
                    break;
                case 2:
                    Console.WriteLine("새로운 게임을 시작합니다.");
                    player.SetPlayer();  // 새로운 게임 시작
                    break;

            }

        }
        else
        {
            Console.WriteLine("저장된 게임 파일이 없습니다. 새로운 게임을 시작합니다.");
            player.SetPlayer();  // 새로운 게임 시작
            shop = new Shop(player);
        }

        // 게임 진행
        Town town = new Town();
        shop ??= new Shop(player);
        Dungeon dungeon = new Dungeon();
        Rest rest = new Rest();

        Console.Clear(); // 콘솔 화면 정리 (선택사항)
        town.TownMap(player, inventory, shop, dungeon, rest, quest); // 마을 지도
    }
}

