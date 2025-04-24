using System.Runtime.InteropServices;
using TextRpg;
using System.Text;

internal class Program
{
    internal static Player player = new Player();
    internal static Inventory inventory = new Inventory();
    internal static Quest quest = new Quest();
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.SetWindowSize(150, 60);

        Town town = new Town();
        Shop shop = new Shop(player);
        Dungeon dungeon = new Dungeon();
        Rest rest = new Rest();
        
        player.SetPlayer();
        Console.Clear(); // 콘솔 화면 정리 (선택사항)
        town.TownMap(player, inventory, shop, dungeon, rest);
    }
}
