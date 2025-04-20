using System.Text;
using SpartaDungeon.Core.Data;
using SpartaDungeon.Core.Dungeon;
using SpartaDungeon.Core.Dungeon.Interface;
using SpartaDungeon.Core.Equipment;
using SpartaDungeon.Core.Equipment.Interface;
using SpartaDungeon.Core.Inventory;
using SpartaDungeon.Core.Inventory.Interface;
using SpartaDungeon.Core.Shop;
using SpartaDungeon.Core.Shop.Interface;
using SpartaDungeon.Managers;
using SpartaDungeon.User;

namespace SpartaDungeon
{
    public class DependencyContainer
    {        public static GameManager CreateGameManager()
        {
            return new GameManager();
        }
        
        public static IPlayerInventory CreateInventory()
        {
            return new PlayerInventory();
        }
        
        public static IPlayerEquipmentSlot CreateEquipmentSlot() 
        {
            return new PlayerEquipmentSlot();
        }
        
        public static Player CreatePlayer(IPlayerInventory inventory, IPlayerEquipmentSlot equipmentSlot)
        {
            return new Player(inventory, equipmentSlot);
        }
        
        public static IDungeonManager CreateDungeonManager(Player player)
        {
            return new DungeonManager(player);
        }
        
        public static IItemShop CreateShop()
        {
            return new ItemShop();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            //Item sword = new Item("Sword", "A sharp blade.", 100);
            //Item shield = new Item("Shield", "A sturdy shield.", 150);
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(140, 60);

            // 아이템 데이터베이스 초기화
            ItemDataBase itemDataBase = ItemDataBase.Instance;
            
            // 게임 시작 - 게임 루프 시작
            GameManager.Instance.StartGame();
        }
    }
}
