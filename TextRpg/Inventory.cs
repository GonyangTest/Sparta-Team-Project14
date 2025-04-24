using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TextRpg
{
    class Inventory
    {
        public List<Item> inventory;

        // Shop을 받지 않도록 수정 (굳이 Shop을 인자로 받지 않음)
        public Inventory()
        {
            inventory = new List<Item>();
        }

        public void ShowInventory(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < inventory.Count; i++)
                {
                    Item item = inventory[i];

                    string equipTag = "";
                    if (item == player.EquippedWeapon || item == player.EquippedArmor) // 장착 여부 확인
                        equipTag = "[E]";

                    Console.Write($"- {i + 1} {equipTag}{item.itemName.PadRight(12)} | ");

                    if (item is Weapon weapon)
                        Console.Write($"공격력 +{weapon.attack} | ");
                    else if (item is Armor armor)
                        Console.Write($"방어력 +{armor.defense} | ");

                    Console.WriteLine($"{item.description}");
                }


                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowEquipMenu(player);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void ShowEquipMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리\n보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < inventory.Count; i++)
                {
                    var item = inventory[i];
                    string equippedMark = "";

                    if (item == player.EquippedWeapon || item == player.EquippedArmor)
                        equippedMark = "[E]";

                    string stat = item is Weapon w ? $"공격력 +{w.attack}" :
                                  item is Armor a ? $"방어력 +{a.defense}" :
                                  "";

                    Console.WriteLine($"- {i + 1} {equippedMark}{item.itemName,-12} | {stat} | {item.description}");
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("숫자만 입력해주세요.");
                    Console.ReadKey();
                    continue;
                }

                if (choice == 0)
                    break;

                if (choice < 1 || choice > inventory.Count)
                {
                    Console.WriteLine("올바른 번호를 입력해주세요.");
                    Console.ReadKey();
                    continue;
                }

                Item selected = inventory[choice - 1];

                if (selected is Weapon)
                {
                    if (player.EquippedWeapon == selected)
                    {
                        player.EquippedWeapon = null;
                        Console.WriteLine($"{selected.itemName} 을(를) 해제했습니다! (무기)");
                    }
                    else
                    {
                        player.EquippedWeapon = selected;
                        Console.WriteLine($"{selected.itemName} 을(를) 장착했습니다! (무기)");
                        Program.quest.QuestRenewal(1, 1); // 장비 장착 퀘스트 판정
                    }
                    //player.CurrentPlayer(); // 능력치 갱신
                }
                else if (selected is Armor)
                {
                    if (player.EquippedArmor == selected)
                    {
                        player.EquippedArmor = null;
                        Console.WriteLine($"{selected.itemName} 을(를) 해제했습니다! (방어구)");
                    }
                    else
                    {
                        player.EquippedArmor = selected;
                        Console.WriteLine($"{selected.itemName} 을(를) 장착했습니다! (방어구)");
                        Program.quest.QuestRenewal(1, 1); // 장비 장착 퀘스트 판정
                    }
                    //player.CurrentPlayer(); // 능력치 갱신
                }
                else
                {
                    Console.WriteLine("장착할 수 없는 아이템입니다.");
                }

                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }
        public void RemoveItem(Item item)
        {
            if (inventory.Contains(item))
            {
                inventory.Remove(item);
            }
        }
        public List<Item> GetOwnedItems()
        {
            // 구매한 아이템만 리턴
            return inventory.Where(i => i.isPurchased).ToList();
        }

    }
}
