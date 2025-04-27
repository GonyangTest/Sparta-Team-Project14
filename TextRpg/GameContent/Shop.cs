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

                // FigletText를 사용하여 큰 타이틀 텍스트 생성 및 금색으로 설정
                var title = new FigletText("상점")
                    .Color(Color.Gold1);
                AnsiConsole.Write(title);

                // Rule로 꾸며진 구분선 생성
                AnsiConsole.Write(new Rule("[yellow]필요한 아이템을 얻을 수 있는 상점입니다[/]").RuleStyle("gold1"));
                AnsiConsole.WriteLine();

                // Panel을 사용해 골드 정보를 테두리가 있는 박스에 표시
                AnsiConsole.Markup($"[[보유 골드]] : [gold1]{_player.gold}G[/]");

                // Table 컴포넌트로 아이템 목록을 테이블 형식으로 표시
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Gold1)
                    .Title("[gold1]아이템 목록[/]")
                    .AddColumn(new TableColumn("[yellow]이름[/]").Centered())
                    .AddColumn(new TableColumn("[yellow]종류[/]").Centered())
                    .AddColumn(new TableColumn("[yellow]효과[/]").Centered())
                    .AddColumn(new TableColumn("[yellow]가격[/]").Centered())
                    .AddColumn(new TableColumn("[yellow]설명[/]")) // 설명 열 추가
                    .AddColumn(new TableColumn("[yellow]상태[/]").Centered());

                // 각 아이템을 테이블 행으로 추가
                foreach (Item item in _items.Where(i => i != null))
                {
                    string itemType = item is ConsumableItem ? "소비품" :
                                    item is Weapon ? "무기" : "방어구";

                    string effect = "";
                    if (item is Weapon weapon)
                        effect = $"+공격력 {weapon._attack}";
                    else if (item is Armor armor)
                        effect = $"+방어력 {armor._defense}";
                    else if (item is ConsumableItem consumable)
                        effect = consumable.ItemName.Contains("체력") ? "HP 회복" : "MP 회복";

                    string status = item.IsPurchased ? "[red]구매완료[/]" : "[green]구매가능[/]";

                    table.AddRow(
                        $"[cyan]{item.ItemName}[/]",
                        itemType,
                        effect,
                        $"[gold1]{item.Price} G[/]",
                        $"[grey]{item.Description}[/]", // 설명 추가
                        status
                    );
                }

                AnsiConsole.Write(table);
                AnsiConsole.WriteLine();

                // SelectionPrompt로 메뉴 선택 UI 생성
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("원하시는 [green]행동[/]을 선택해주세요.")
                        .PageSize(10)
                        .HighlightStyle(new Style().Foreground(Color.Gold1)) // 선택 항목 강조 색상
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
                        SellItem(inventory);
                        break;
                }
            }
        }

        public void DisplayBuyItems(Inventory inventory)
        {
            while (true)
            {
                Console.Clear();

                // 구매 화면 타이틀 (초록색으로 설정)
                var title = new FigletText("구매")
                    .Color(Color.Green);
                AnsiConsole.Write(title);

                // 구분선 추가
                AnsiConsole.Write(new Rule("[green]번호를 눌러 아이템을 구매하세요[/]"));
                AnsiConsole.WriteLine();

                // 골드 정보 표시
                AnsiConsole.Markup($"[[보유 골드]] : [gold1]{_player.gold}G[/]");

                // 아이템 선택 프롬프트
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<Item>()
                        .Title("[green]구매하려는 아이템을 선택해주세요.[/]")
                        .PageSize(10)
                        .HighlightStyle(new Style().Foreground(Color.Green))
                        .AddChoices(_items)
                        .UseConverter(item => {
                            // 각 아이템을 표시 형식으로 변환
                            if (item == null) return "[blue]뒤로가기[/]";

                            string status = item.IsPurchased ? "[red]구매완료[/]" : "[green]구매가능[/]";
                            return $"[cyan]{item.ItemName}[/] - [gold1]{item.Price} G[/] {status}";
                        })
                        .WrapAround());

                Item selectedItem = choice;

                if (selectedItem == null)
                    break;

                // 소비 아이템(포션) 구매 처리
                if (selectedItem is ConsumableItem)
                {
                    if (_player.gold >= selectedItem.Price)
                    {
                        _player.gold -= selectedItem.Price;

                        if (selectedItem.ItemName.Contains("체력"))
                            _player.HealthPotion.Quantity++;
                        else
                            _player.ManaPotion.Quantity++;

                        // 구매 성공 메시지 (색상 적용)
                        AnsiConsole.MarkupLine($"[green]{selectedItem.ItemName}을(를) 구매했습니다![/]");
                        AnsiConsole.Write(new Markup($"[grey]현재 골드: [gold1]{_player.gold} G[/][/]"));
                    }
                    else
                    {
                        AnsiConsole.Write(new Markup("[red]골드가 부족합니다![/]"));
                    }
                }
                else if (selectedItem.IsPurchased)
                {
                    AnsiConsole.Write(new Markup("[yellow]이미 구매한 아이템입니다.[/]"));
                }
                else if (_player.gold >= selectedItem.Price)
                {
                    _player.gold -= selectedItem.Price;
                    selectedItem.IsPurchased = true;
                    inventory.getInventory().Add(selectedItem);

                    // 구매 과정 애니메이션 효과
                    AnsiConsole.Status()
                        .Spinner(Spinner.Known.Star) // 별 모양 스피너
                        .SpinnerStyle(Style.Parse("green"))
                        .Start("구매 처리 중...", ctx => {
                            Thread.Sleep(800); // 처리 중인 효과를 위한 지연
                        });

                    AnsiConsole.MarkupLine($"[green]{selectedItem.ItemName}을(를) 구매했습니다![/]");
                    AnsiConsole.Write(new Markup($"[grey]현재 골드: [gold1]{_player.gold} G[/][/]"));
                }
                else
                {
                    AnsiConsole.Write(new Markup("[red]골드가 부족합니다![/]"));
                }

                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Rule("[blue]계속하려면 아무 키나 누르세요...[/]"));
                Console.ReadKey();
            }
        }

        public void SellItem(Inventory inventory)
        {
            var ownedItems = inventory.GetOwnedItems();

            if (ownedItems.Count == 0)
            {
                // 판매할 아이템이 없는 경우 빨간색 패널로 표시
                AnsiConsole.Write(new Panel("[yellow]판매할 아이템이 없습니다.[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Red));
                Console.ReadKey();
                return;
            }

            Console.Clear();

            // 판매 화면 타이틀 (빨간색으로 설정)
            var title = new FigletText("판매")
                .Color(Color.Red);
            AnsiConsole.Write(title);

            AnsiConsole.Write(new Rule("[red]번호를 입력해 아이템을 판매하세요[/]"));
            AnsiConsole.WriteLine();

            // 골드 정보 표시
            AnsiConsole.Markup($"[[보유 골드]] : [gold1]{_player.gold}G[/]");

            ownedItems.Add(null); // 뒤로가기 옵션 추가

            // 판매할 아이템 선택 프롬프트
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Item>()
                    .Title("[red]판매하려는 아이템을 선택해주세요.[/]")
                    .PageSize(10)
                    .HighlightStyle(new Style().Foreground(Color.Red))
                    .AddChoices(ownedItems)
                    .UseConverter(item => {
                        // 각 아이템 정보에 색상과 아이콘 추가
                        if (item == null) return "[blue]뒤로가기[/]";

                        int sellPrice = (int)(item.Price * 0.85);
                        string itemType = item is Weapon ? "[orange3](무기)[/]" :
                                         item is Armor ? "[blue](방어구)[/]" : "[green](소비품)[/]";

                        return $"[cyan]{item.ItemName}[/] {itemType} | 판매가격: [gold1]{sellPrice} G[/]";
                    })
                    .WrapAround());

            Item selectedItem = choice;

            if (selectedItem != null)
            {
                int sellPrice = (int)(selectedItem.Price * 0.85);

                // 판매 정보를 테이블로 표시
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Red)
                    .Title("[red]판매 정보[/]")
                    .AddColumn("[yellow]항목[/]")
                    .AddColumn("[yellow]내용[/]");

                table.AddRow("아이템", $"[cyan]{selectedItem.ItemName}[/]");
                table.AddRow("판매가격", $"[gold1]{sellPrice} G[/]");

                AnsiConsole.Write(table);

                // 판매 전 확인 프롬프트
                if (AnsiConsole.Confirm("정말로 판매하시겠습니까?"))
                {
                    // 아이템이 장착 중이라면 해제
                    if (selectedItem == _player.EquippedWeapon)
                    {
                        _player.EquippedWeapon = null;
                        AnsiConsole.MarkupLine($"[yellow]{selectedItem.ItemName}을(를) 해제했습니다. (무기)[/]");
                    }
                    else if (selectedItem == _player.EquippedArmor)
                    {
                        _player.EquippedArmor = null;
                        AnsiConsole.MarkupLine($"[yellow]{selectedItem.ItemName}을(를) 해제했습니다. (방어구)[/]");
                    }

                    // 상점 아이템 목록에서 해당 아이템 찾아 구매 상태 초기화
                    Item item = _items.Find(i => i?.ItemName == selectedItem.ItemName);
                    if (item != null)
                    {
                        item.IsPurchased = false;
                    }

                    // 판매 처리 애니메이션
                    AnsiConsole.Status()
                        .Spinner(Spinner.Known.Dots) // 점 애니메이션
                        .SpinnerStyle(Style.Parse("red"))
                        .Start("판매 중...", ctx => {
                            Thread.Sleep(1000); // 판매 처리 중 효과
                        });

                    // 아이템 판매 처리
                    inventory.RemoveItem(selectedItem);
                    _player.gold += sellPrice;

                    // 판매 성공 결과 메시지
                    AnsiConsole.MarkupLine($"[green]{selectedItem.ItemName}을(를) {sellPrice} G에 판매했습니다![/]");
                    AnsiConsole.MarkupLine($"[grey]현재 보유 골드: [gold1]{_player.gold} G[/][/]");
                }
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[blue]계속하려면 아무 키나 누르세요...[/]"));
            Console.ReadKey();
        }

        public List<Item> GetItems()
        {
            return _items;
        }

    }
}
