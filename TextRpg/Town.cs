using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    class Town
    {
        public void TownMap(Player player, Inventory inventory, Shop shop, Dungeon dungeon, Rest rest)
        {
            bool isExit = true;
            while (isExit)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n5. 휴식하기\n6. 퀘스트\n0. 게임종료\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                string townSelect = Console.ReadLine();
                int townTmp;
                if (!int.TryParse(townSelect, out townTmp))
                {
                    Console.Clear();
                    Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                    Console.ReadKey();
                }
                else
                {
                    switch (townTmp)
                    {
                        case 0:
                            Console.Clear();
                            isExit = false;
                            Console.WriteLine("게임종료");
                            break;
                        case 1:
                            Console.Clear();
                            Console.WriteLine(player.PrintPlayer());
                            Console.WriteLine("\n0. 나가기");
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
                        default:
                            Console.Clear();
                            Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
        
    }
}
