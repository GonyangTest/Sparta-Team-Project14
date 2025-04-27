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
            List<string> menu = new List<string>
            {
                "1. 장착 관리",
                "0. 나가기"
            };

            while (true)
            {
                Console.Clear();
                AnsiConsole.Write(new Rule("[green]보유중인 아이템을 관리할수 있는 인벤토리입니다[/]"));

                for (int i = 0; i < _inventory.Count; i++)
                {
                    Item item = _inventory[i];

                    string equipTag = "";
                    if (item == player.EquippedWeapon || item == player.EquippedArmor) // 장착 여부 확인
                        equipTag = "[E]";

                    Console.Write($"- {equipTag}{item.ItemName.PadRight(12)} | ");

                    if (item is Weapon weapon)
                        Console.Write($"공격력 +{weapon.Attack} | ");
                    else if (item is Armor armor)
                        Console.Write($"방어력 +{armor.Defense} | ");

                    Console.WriteLine($"{item.Description}");
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
                    case 1:
                        ShowEquipMenu(player);
                        break;
                    case 0:
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
                AnsiConsole.Write(new Rule("[red]장착/해제[/][green]할 아이템을 선택하세요[/]"));

                // 취소 옵션을 포함하기 위해 아이템 목록에 null 추가
                var itemChoices = new List<Item>(_inventory);
                itemChoices.Add(null); // 나가기 옵션
                
                // Spectre.Console의 SelectionPrompt 사용
                var selectedItem = AnsiConsole.Prompt(
                    new SelectionPrompt<Item>()
                        .PageSize(10)
                        .AddChoices(itemChoices)
                        .WrapAround()
                        .UseConverter(item => {
                            if (item == null) return "0. 나가기";
                            
                            // 인덱스 계산
                            int itemIndex = _inventory.IndexOf(item) + 1;
                            
                            string equippedMark = "";
                            if (item == player.EquippedWeapon || item == player.EquippedArmor)
                                equippedMark = "[[E]] ";
                            
                            string stat = item is Weapon w ? $"공격력 +{w.Attack}" :
                                        item is Armor a ? $"방어력 +{a.Defense}" :
                                        "";
                            
                            return $"{itemIndex}. {equippedMark}{item.ItemName,-12} | {stat} | {item.Description}";
                        })
                );
                
                // 나가기 선택 처리
                if (selectedItem == null)
                    break;
                    
                // 아이템 장착/해제 처리
                if (selectedItem is Weapon weapon)
                {
                    if (player.EquippedWeapon == selectedItem)
                    {
                        // 해제 과정 애니메이션 효과
                        AnsiConsole.Status()
                            .Spinner(Spinner.Known.Star) // 별 모양 스피너
                            .SpinnerStyle(Style.Parse("yellow"))
                            .Start("장비 해제 중...", ctx => {
                                Thread.Sleep(800); // 처리 중인 효과를 위한 지연
                            });
                        player.EquippedWeapon = null;
                        AnsiConsole.MarkupLine($"[green]{selectedItem.ItemName}[/] 을(를) 해제했습니다! (무기)");
                    }
                    else
                    {
                        // 장착 과정 애니메이션 효과
                        AnsiConsole.Status()
                            .Spinner(Spinner.Known.Star) // 별 모양 스피너
                            .SpinnerStyle(Style.Parse("yellow"))
                            .Start("장비 장착 중...", ctx => {
                                Thread.Sleep(800); // 처리 중인 효과를 위한 지연
                            });
                        player.EquippedWeapon = weapon;
                        AnsiConsole.MarkupLine($"[green]{selectedItem.ItemName}[/] 을(를) 장착했습니다! (무기)");
                        Program.quest.QuestRenewal(1, 1); // 장비 장착 퀘스트 판정
                    }
                }
                else if (selectedItem is Armor armor)
                {
                    if (player.EquippedArmor == selectedItem)
                    {
                        // 해제 과정 애니메이션 효과
                        AnsiConsole.Status()
                            .Spinner(Spinner.Known.Star) // 별 모양 스피너
                            .SpinnerStyle(Style.Parse("yellow"))
                            .Start("장비 해제 중...", ctx => {
                                Thread.Sleep(800); // 처리 중인 효과를 위한 지연
                            });
                        player.EquippedArmor = null;
                        AnsiConsole.MarkupLine($"[green]{selectedItem.ItemName}[/] 을(를) 해제했습니다! (방어구)");
                    }
                    else
                    {
                        // 장착 과정 애니메이션 효과
                        AnsiConsole.Status()
                            .Spinner(Spinner.Known.Star) // 별 모양 스피너
                            .SpinnerStyle(Style.Parse("yellow"))
                            .Start("장비 장착 중...", ctx => {
                                Thread.Sleep(800); // 처리 중인 효과를 위한 지연
                            });
                        player.EquippedArmor = armor;
                        AnsiConsole.MarkupLine($"[green]{selectedItem.ItemName}[/] 을(를) 장착했습니다! (방어구)");
                        Program.quest.QuestRenewal(1, 1); // 장비 장착 퀘스트 판정
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
