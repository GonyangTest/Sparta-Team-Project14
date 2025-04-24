using System.Runtime.InteropServices;
using TextRpg;

internal class Program
{
    static void Main(string[] args)
    {
        Player player = new Player();
        Town town = new Town();
        Inventory inventory = new Inventory();
        Shop shop = new Shop(player);
        Dungeon dungeon = new Dungeon();
        Rest rest = new Rest();
        


        player.SetPlayer();
        Console.Clear(); // 콘솔 화면 정리 (선택사항)
        town.TownMap(player, inventory, shop, dungeon, rest);

    }
}
