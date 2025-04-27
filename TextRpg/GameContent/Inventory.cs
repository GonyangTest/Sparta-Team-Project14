using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Spectre.Console;

namespace TextRpg
{
    class Inventory
    {
        private List<Item> _inventory;

        public List<Item> getInventory() { return _inventory; }

        // Shop을 받지 않도록 수정 (굳이 Shop을 인자로 받지 않음)
        public Inventory()
        {
            _inventory = new List<Item>();
        }

        public void ShowInventory(Player player)
        {
            bool isRunning = true;
            List<string> menu = new List<string>
            {
                "1. 장착 관리",
                "0. 나가기"
            };

            while (isRunning)
            {
                Console.Clear();

                // FigletText를 사용하여 큰 타이틀 텍스트 생성 및 청록색으로 설정
                var title = new FigletText("인벤토리")
                    .Color(Color.Gold1);
                AnsiConsole.Write(title);

                // Rule로 꾸며진 구분선 생성
                AnsiConsole.Write(new Rule("[yellow]보유중인 아이템을 관리할 수 있는 인벤토리입니다[/]").RuleStyle("gold1"));
                AnsiConsole.WriteLine();

                // 장착 중인 장비 상태 표시
                var equipStatus = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Gold1)
                    .Title("[Gold1]장착 상태[/]")
                    .AddColumn(new TableColumn("[yellow]슬롯[/]").Centered())
                    .AddColumn(new TableColumn("[yellow]아이템[/]").Centered())
                    .AddColumn(new TableColumn("[yellow]효과[/]").Centered());

                string weaponName = player.EquippedWeapon != null ? player.EquippedWeapon.ItemName : "없음";
                string armorName = player.EquippedArmor != null ? player.EquippedArmor.ItemName : "없음";
                string weaponEffect = player.EquippedWeapon != null ? $"+공격력 {(player.EquippedWeapon as Weapon).Attack}" : "";
                string armorEffect = player.EquippedArmor != null ? $"+방어력 {(player.EquippedArmor as Armor).Defense}" : "";

                equipStatus.AddRow("[cyan]무기[/]", $"[yellow]{weaponName}[/]", weaponEffect);
                equipStatus.AddRow("[cyan]방어구[/]", $"[yellow]{armorName}[/]", armorEffect);

                AnsiConsole.Write(equipStatus);
                AnsiConsole.WriteLine();

                // 인벤토리 아이템 목록을 테이블 형식으로 표시
                if (_inventory.Count > 0)
                {
                    var itemTable = new Table()
                        .Border(TableBorder.Rounded)
                        .BorderColor(Color.Gold1)
                        .Title("[Gold1]보유 아이템[/]")
                        .AddColumn(new TableColumn("[yellow]이름[/]").Centered())
                        .AddColumn(new TableColumn("[yellow]종류[/]").Centered())
                        .AddColumn(new TableColumn("[yellow]효과[/]").Centered())
                        .AddColumn(new TableColumn("[yellow]설명[/]"));

                    foreach (Item item in  _inventory)
                    {
                        string itemType = "";
                        string effect = "";
                        if (item is Weapon weapon){
                            itemType = "무기";
                            effect = $"+공격력 {weapon.Attack}";
                        }
                        else if (item is Armor armor){
                            itemType = "방어구";
                            effect = $"+방어력 {armor.Defense}";
                        }
                        else if (item is ConsumableItem consumable){
                            itemType = "소비품";
                            effect = consumable.ItemName.Contains("체력") ? "HP 회복" : "MP 회복";
                        }
 
                        string itmeName = $"[yellow]{item.ItemName}[/]";
                        if(item == player.EquippedWeapon || item == player.EquippedArmor)
                            itmeName = $"[green][[E]][/]{itmeName}";

                        itemTable.AddRow(
                            itmeName,
                            itemType,
                            effect,
                            $"[grey]{item.Description}[/]"
                        );
                    }

                    AnsiConsole.Write(itemTable);
                }
                else
                {
                    // 아이템이 없는 경우
                    AnsiConsole.Write(new Panel("[yellow]보유한 아이템이 없습니다.[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Gold1));
                }

                AnsiConsole.WriteLine();

                // SelectionPrompt로 메뉴 선택 UI 생성
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("원하시는 [green]행동[/]을 선택해주세요.")
                        .PageSize(10)
                        .HighlightStyle(new Style().Foreground(Color.Gold1))
                        .AddChoices(menu)
                        .WrapAround());

                int index = int.Parse(choice.Split('.')[0]);

                switch (index)
                {
                    case 1:
                        ShowEquipMenu(player);
                        break;
                    case 0:
                        isRunning = false;
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]잘못된 입력입니다.[/]");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void ShowEquipMenu(Player player)
        {
            if (_inventory.Count == 0)
            {
                AnsiConsole.Write(new Panel("[yellow]장착할 아이템이 없습니다. 상점에서 아이템을 구매해보세요.[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Red));
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();;
                // 구분선 추가
                AnsiConsole.Write(new Rule("[yellow]장착하거나 해제할 아이템을 선택하세요[/]"));
                AnsiConsole.WriteLine();

                string weaponName = player.EquippedWeapon != null ? $"[cyan]{player.EquippedWeapon.ItemName}[/]" : "[grey]없음[/]";
                string armorName = player.EquippedArmor != null ? $"[cyan]{player.EquippedArmor.ItemName}[/]" : "[grey]없음[/]";

                // 현재 장착 상태 정보 표시 (패널로 표시)
                var equipPanel = new Panel($"[yellow]무기[/]: {weaponName} | [yellow]방어구[/]: {armorName}")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Yellow)
                    .Header("[yellow]현재 장착 상태[/]");
                AnsiConsole.Write(equipPanel);
                AnsiConsole.WriteLine();


                var itemChoices = new List<Item>(_inventory);
                itemChoices.Add(null); // 뒤로가기 옵션
                
                // Spectre.Console의 SelectionPrompt 사용
                var selectedItem = AnsiConsole.Prompt(
                    new SelectionPrompt<Item>()
                        .Title("[Green]장착/해제할 아이템을 선택해주세요.[/]")
                        .PageSize(10)
                        .HighlightStyle(new Style().Foreground(Color.Green))
                        .AddChoices(itemChoices)
                        .UseConverter(item => {
                            if (item == null) return "[blue]뒤로가기[/]";
                            
                            string equippedMark = "";
                            if (item == player.EquippedWeapon || item == player.EquippedArmor)
                                equippedMark = "[[E]] ";
                            
                            string itemType = item is Weapon ? "[orange3](무기)[/]" :
                                            item is Armor ? "[blue](방어구)[/]" : "[green](소비품)[/]";
                            
                            string stat = item is Weapon w ? $"공격력 +{w.Attack}" :
                                        item is Armor a ? $"방어력 +{a.Defense}" :
                                        "";
                            
                            return $"[cyan]{equippedMark}{item.ItemName}[/] {itemType} | {stat}";
                        })
                        .WrapAround()
                );
                
                // 나가기 선택 처리
                if (selectedItem == null)
                    break;
                    
                // 아이템 장착/해제 처리
                if (selectedItem is Weapon weapon)
                {
                    if (player.EquippedWeapon == selectedItem)
                    {
                        // 아이템 해제
                        if (AnsiConsole.Confirm("정말로 해제하시겠습니까?"))
                            UnequipItem(player, selectedItem, "무기");
                    }
                    else
                    {
                        // 무기 장착
                        if (AnsiConsole.Confirm("정말로 장착하시겠습니까?"))
                            EquipItem(player, weapon, isWeapon: true);
                    }
                }
                else if (selectedItem is Armor armor)
                {
                    if (player.EquippedArmor == selectedItem)
                    {
                        // 아이템 해제
                        if (AnsiConsole.Confirm("정말로 해제하시겠습니까?"))
                            UnequipItem(player, selectedItem, "방어구");
                    }
                    else
                    {
                        // 방어구 장착
                        if (AnsiConsole.Confirm("정말로 장착하시겠습니까?"))
                            EquipItem(player, armor, isWeapon: false);
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]장착할 수 없는 아이템입니다.[/]");
                }

                AnsiConsole.Write(new Rule("[blue]계속하려면 아무 키나 누르세요...[/]"));
                Console.ReadKey();
            }
        }

        // 장비 해제 메소드
        private void UnequipItem(Player player, Item item, string itemType)
        {
            // 해제 과정 애니메이션 효과
            ShowEquipAnimation("장비 해제 중...");
            
            // 장비 유형에 따라 해제 처리
            if (itemType == "무기")
                player.EquippedWeapon = null;
            else if (itemType == "방어구")
                player.EquippedArmor = null;
                
            AnsiConsole.MarkupLine($"[green]{item.ItemName}[/] 을(를) 해제했습니다! ({itemType})");
        }
        
        // 장비 장착 메소드
        private void EquipItem(Player player, Item item, bool isWeapon)
        {
            // 장착 과정 애니메이션 효과
            ShowEquipAnimation("장비 장착 중...");
            
            // 장비 유형에 따라 장착 처리
            if (isWeapon)
                player.EquippedWeapon = item as Weapon;
            else
                player.EquippedArmor = item as Armor;
                
            AnsiConsole.MarkupLine($"[green]{item.ItemName}[/] 을(를) 장착했습니다! ({(isWeapon ? "무기" : "방어구")})");
            Program.quest.QuestRenewal(1, 1); // 장비 장착 퀘스트 판정
        }
        
        // 장착 애니메이션 표시
        private void ShowEquipAnimation(string message)
        {
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("yellow"))
                .Start(message, ctx => {
                    Thread.Sleep(GameConstance.Inventory.ITEM_EQUIP_SLEEP);
            });
        }

        public void RemoveItem(Item item)
        {
            if (_inventory.Contains(item))
            {
                _inventory.Remove(item);
            }
        }
        
        public List<Item> GetOwnedItems()
        {
            // 구매한 아이템만 리턴
            return _inventory.Where(i => i.IsPurchased).ToList();
        }
    }
}
