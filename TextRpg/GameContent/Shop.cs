using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRpg;
using Spectre.Console;

namespace TextRpg
{
    class Shop
    {
        private Player _player;
        private List<Item> _items;

        public Shop(Player player)
        {
            _player = player;

            _items = new List<Item>();
            _items = ItemFactory.GetAllItems();
            _items.Add(null);

        }
        public void DisplayItems(Inventory inventory)
        {
            bool isRunning = true;
            List<string> menu = new List<string>()
            {
                "1. 아이템 구매",
                "2. 아이템 판매",
                "0. 나가기"
            };
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"[보유 골드]\n{_player.gold} G\n");
                Console.WriteLine("[아이템 목록]\n");
                foreach (Item item in _items)
                {
                    if (item != null)
                        Console.WriteLine(item.GetInfo());
                }

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("원하시는 [green]행동[/]을 선택해주세요.")
                        .PageSize(10)
                        .AddChoices(menu)
                        .WrapAround());

                int index = int.Parse(choice.Split('.')[0]);

                switch (index)
                {
                    case 0:
                        isRunning = false;
                        break;
                    case 1:
                        DisplayBuyItems(inventory);
                        break;
                    case 2:
                        SellItem(inventory); // 아이템 판매
                        break;
                }
            }
        }
        public void DisplayBuyItems(Inventory inventory)
        {
            while (true) // 상점 메뉴를 계속 유지하려면 무한 루프
            {
                Console.Clear();
                Console.WriteLine("상점\n번호를 눌러 아이템을 구매하세요.\n");
                Console.WriteLine($"[보유 골드]\n{_player.gold} G\n");
                Console.WriteLine("[아이템 목록]");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<Item>()
                        .Title("구매하려는 아이템을 선택해주세요.")
                        .PageSize(10)
                        .AddChoices(_items)
                        .WrapAround()
                        .UseConverter(item => item == null ? "뒤로가기" : $"- {item.GetInfo()}"));

                Item  selectedItem = choice; // items리스트의 타입이 Item이므로 Item형의 변수를 생성하고 입력받은 값을 items 리스트의 인덱스에 접근시킨다.

                if (selectedItem is ConsumableItem) // 소비 아이템(포션)인 경우 isPurchased 상관없이 구매 가능
                {
                    if (_player.gold >= selectedItem.Price) // 플레이어가 가지고 있는 골드가 아이템 가격보다 많이 가지고 있을 경우
                    {
                        _player.gold -= selectedItem.Price; // 플레이어 골드에 아이템 가격을 빼준다.
                        if (selectedItem.ItemName.Contains("체력"))
                        {
                            _player.HealthPotion.Quantity++;
                        }
                        else
                        {
                            _player.ManaPotion.Quantity++;
                        }
                        Console.WriteLine($"{selectedItem.ItemName} 을(를) 구매했습니다!");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다!");
                    }
                }
                else if (selectedItem == null) break;
                else if (selectedItem.IsPurchased) // 선택한 아이템(선택한 리스트의 인덱스)가 이미 구매했는지에 대한 여부를 확인하는 조건문
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else if (_player.gold >= selectedItem.Price) // 또는 플레이어가 가지고있는 골드가 아이템 가격보다 많이가지고 있을경우(같을경우)
                {
                    _player.gold -= selectedItem.Price; // 플레이어 골드에 아이템 가격을 빼준다.
                    selectedItem.IsPurchased = true; // 구매를 했으니 구매했는지에 대한 여부를 나타내는 bool타입 변수를 true로 바꿔줘 중복구매를 방지한다.
                    inventory.getInventory().Add(selectedItem); // 인벤토리에 추가
                    Console.WriteLine($"{selectedItem.ItemName} 을(를) 구매했습니다!");
                }
                else // 그외는 골드가 모자를 경우
                {
                    Console.WriteLine("골드가 부족합니다!");
                }

                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }
        public void SellItem(Inventory inventory)
        {
            var ownedItems = inventory.GetOwnedItems();

            if (ownedItems.Count == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.");
                Console.ReadKey();
                return;
            }



            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매\n");
            Console.WriteLine("번호를 입력해 아이템을 판매하세요.\n");
            Console.WriteLine($"\n[보유 골드]\n{_player.gold} G");
            ownedItems.Add(null);


            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Item>()
                    .Title("판매하려는 아이템을 선택해주세요.")
                    .PageSize(10)
                    .AddChoices(ownedItems)
                    .WrapAround()
                    .UseConverter(item => item == null ? "뒤로가기" : $"- {item.GetInfo()} | 가격 : {(int)(item.Price * 0.85)}"));

            Item selectedItem = choice;
            if (selectedItem != null)
            {
                // 아이템이 장착 중이라면 해제
                if (selectedItem == _player.EquippedWeapon)
                {
                    _player.EquippedWeapon = null;
                    Console.WriteLine($"{selectedItem.ItemName}을(를) 해제했습니다. (무기)");
                }
                else if (selectedItem == _player.EquippedArmor)
                {
                    _player.EquippedArmor = null;
                    Console.WriteLine($"{selectedItem.ItemName}을(를) 해제했습니다. (방어구)");
                }

                Item? item = _items.Find(item => item.ItemName == selectedItem.ItemName);
                if (item != null)
                {
                    item.IsPurchased = false;
                }

                // 아이템 판매 처리
                inventory.RemoveItem(selectedItem); // 아이템을 인벤토리에서 제거
                int sellPrice = (int)(selectedItem.Price * 0.85); // 85% 가격
                _player.gold += sellPrice; // 골드 증가

                Console.WriteLine($"{selectedItem.ItemName}을(를) {sellPrice} G에 판매했습니다!");
                Console.WriteLine($"현재 보유 골드: {_player.gold} G");

            }

        }
        public List<Item> GetItems()
        {
            return _items;
        }

    }
}
