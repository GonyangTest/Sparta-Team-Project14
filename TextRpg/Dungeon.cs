using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using static TextRpg.GameConstance;

namespace TextRpg
{
    class Dungeon
    {
        Random rand = new Random();

        public void EnterDungeonMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("**던전입장**");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 전투 시작");
                Console.WriteLine("0. 나가기\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine(player.PrintPlayer());
                        Console.WriteLine("\n0. 나가기");
                        Console.ReadLine();
                        break;
                    case "2":
                        SoundManager.Instance.StopMusic();
                        EnterDungeon(player);
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
        private void EnterDungeon(Player player)
        {
            SoundManager.Instance.StartDungeonMusic();
            Console.Clear();
            List<Stage> stages = new List<Stage>();
            int[] floor = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            try
            {
                Console.Clear();
                Console.WriteLine("Battle");

                for (int i = 1; i <= floor.Length; i++)
                {
                    stages.Add(new Stage($"{i}단계", normalCount: 1, eliteCount: 1, bossCount: 1));
                }

                foreach (Stage stage in stages)
                {
                    stage.Start(player);
                    if (player.hp <= 0)
                    {
                        Console.WriteLine("플레이어가 사망했습니다. 던전에서 퇴장합니다...");
                        Thread.Sleep(2000);
                        SoundManager.Instance.StopMusic();
                        SoundManager.Instance.StartMainMusic();
                        break;
                    }

                    if (!stage.Cleared)
                    {
                        Console.WriteLine("스테이지 클리어에 실패했습니다...");
                        Thread.Sleep(2000);
                        SoundManager.Instance.StopMusic();
                        SoundManager.Instance.StartMainMusic();
                        break;
                    }

                    // 다음 스테이지로 진행 여부
                    Console.Clear();
                    Console.WriteLine($"현재까지 {stage.Name} 스테이지 클리어!");
                    Console.WriteLine("1. 다음 스테이지 도전");
                    Console.WriteLine("0. 마을로 돌아가기");
                    Console.Write("\n선택 >> ");
                    string input = Console.ReadLine();

                    if (input == "0")
                    {
                        Console.WriteLine("던전을 떠납니다...");
                        Thread.Sleep(1000);
                        SoundManager.Instance.StopMusic();
                        SoundManager.Instance.StartMainMusic();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[에러 발생] {ex.Message}");
                Console.ReadKey();
            }

        }
    }
    public class Stage
    {
        public string Name { get; private set; }//던전이름
        public List<Monster> Monsters { get; private set; }//몬스터 리스트
        public bool Cleared { get; private set; }//클리어유무판독

        private static bool monstersLoaded = false;//

        // 진행바 길이 !!!!!
        int barWidth = 20, partician_Length = 15;

        public Stage(string name, int normalCount, int eliteCount, int bossCount)
        {
            if (!monstersLoaded)
            {
                string filePath = @"..\..\..\monsters.csv";
                //상대경로 수정필요
                MonsterFactory.LoadMonsters(filePath);
                monstersLoaded = true;
            }

            Name = name;

            Monsters = new List<Monster>();

            for (int i = 0; i < normalCount; i++)
                Monsters.Add(MonsterFactory.Create($"1"));

            for (int i = 0; i < eliteCount; i++)
                Monsters.Add(MonsterFactory.Create("2"));

            for (int i = 0; i < bossCount; i++)
                Monsters.Add(MonsterFactory.Create("3"));

            Cleared = false;

            // 진행바 길이에 따라 변동
            partician_Length += barWidth;
        }

        internal void Start(Player player)
        {
            Console.WriteLine($"=== {Name} 스테이지 시작 ===");

            foreach (Monster monster in Monsters)
            {
                Console.WriteLine(monster);

            }

            Battle(player, Monsters);


            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();
        }

        // !!!!!
        private void Battle(Player player, List<Monster> monsters)
        {
            // 몬스터가 다 죽었을 때는 루프를 빠져나가서 보상을 받을 수 있도록 변경
            while (true)
            {
                // Start() 내부에 있던 클리어 판정 구문을 여기로 이동
                // 조건 수정: 플레이어 패배 or 몬스터가 모두 사망한 상태
                if (player.hp <= 0 || Monsters.All(m => m.IsAlive == false))
                {
                    Cleared = true;
                    break;
                }

                Console.Clear();
                Console.WriteLine("Battle!!\n");

                // 몬스터 목록 출력
                for (int i = 0; i < monsters.Count; i++)
                {
                    PrintEnemy(monsters[i], i);
                }

                // 플레이어 출력
                PrintPlayer(player);

                string[] battleMenus = new string[] { "1. 기본 공격", "2. 스킬 공격\n", "0. 도망" };

                var menu = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                           .Title("무엇을 하시겠습니까?")
                           .PageSize(3) // 항목 수
                           .AddChoices(battleMenus)
                           .WrapAround()); // 리스트 순환 >> 맨 위 항목에서 위 방향키를 누르면 제일 아래 항목으로. 역도 성립

                // 선택한 메뉴가 몇번째인가?
                int index = 0;
                foreach (var battleMenu in battleMenus)
                {
                    if (battleMenu == menu)
                        break;
                    else
                        index++;
                }

                // 선택지 선택에 따른 동작
                if (index == 0)
                {
                    BasicAttack(player, monsters);
                    continue;
                }
                else if (index == 1)
                {
                    SkillAttack(player, monsters);
                    continue;
                }
                else if (index == 2)
                    return; // 도망
            }

            // 던전 클리어 결과창
            Result(player, monsters);
        }

        void PrintPlayer(Player player)
        {
            AnsiConsole.MarkupLine($"[#6cf540]{new string('-', partician_Length)}[/]");
            // 플레이어
            AnsiConsole.Markup($"[yellow bold]{player.playerName}[/] ({player.playerClass}) Lv. {player.level}");
            // HP바
            AnsiConsole
            .Progress()
            .Columns(
            // Column의 순서 변경 및 바 색상 변경
                new TaskDescriptionColumn(), // 설명
                new ProgressBarColumn
                {
                    Width = barWidth, // 진행바 길이
                    CompletedStyle = new Style(foreground: Color.Red), // 채워진 부분 색상
                    RemainingStyle = new Style(foreground: Color.Red3), // 채워지지 않은 부분 색상
                    FinishedStyle = new Style(foreground: Color.Red), // 100% 일 때의 색상
                })
                .Start(x =>
                {
                    var task = x.AddTask("[red]HP[/]");
                    float percentage = (float)player.hp * 100 / player.maxHp;
                    task.Increment(percentage);
                });
            Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
            AnsiConsole.MarkupLine($"[red]{player.hp}/{player.maxHp}[/]");

            // MP바
            AnsiConsole
                .Progress()
                .Columns(
                // Column의 순서 변경 및 바 색상 변경
                new TaskDescriptionColumn(), // 설명
                new ProgressBarColumn
                {
                    Width = barWidth, // 진행바 길이
                    CompletedStyle = new Style(foreground: Color.Blue), // 채워진 부분 색상
                    RemainingStyle = new Style(foreground: Color.Blue3), // 채워지지 않은 부분 색상
                    FinishedStyle = new Style(foreground: Color.Blue), // 100% 일 때의 색상
                })
                .Start(x =>
                {
                    var task = x.AddTask("[blue]MP[/]");
                    float percentage = (float)player.mana * 100 / player.maxMp;
                    task.Increment(percentage);
                });
            // 진행바 오른쪽에 커스텀 텍스트를 표시할 방법이 없음.. 그럼 커서 옮기고 써주면 되는 거 아닌가?
            Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
            AnsiConsole.MarkupLine($"[blue]{player.mana}/{player.maxMp}[/]\n");
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
            AnsiConsole.MarkupLine($"[#6cf540]{new string('-', partician_Length)}[/]\n");
        }

        void PrintEnemy(Monster monster, int i)
        {
            var m = monster;
            AnsiConsole.MarkupLine(m.IsAlive ? $"{new string('-', partician_Length)}" : $"[gray]{new string('-', partician_Length)}[/]");
            // 적 정보
            string status = m.IsAlive ? $"{i + 1}. {m.Name} Lv.{m.Level}" : $"[gray]{i + 1}. Lv.{m.Name} {m.Level} (Dead)[/]";
            AnsiConsole.MarkupLine(status);
            // HP바
            AnsiConsole
                .Progress()
                .Columns(
                // Column의 순서 변경 및 바 색상 변경
                new TaskDescriptionColumn(), // 설명
                new ProgressBarColumn
                {
                    Width = barWidth, // 진행바 길이
                    CompletedStyle = new Style(foreground: Color.Red), // 채워진 부분 색상
                    RemainingStyle = m.IsAlive ? new Style(foreground: Color.Red3) : new Style(foreground: Color.FromHex("#a0a0a0")), // 채워지지 않은 부분 색상 + 사망 시 해당 색상이 100% >> 회색
                    FinishedStyle = new Style(foreground: Color.Red), // 100% 일 때의 색상
                })
                .Start(x =>
                {
                    string status = m.IsAlive ? $"[red]HP[/]" : "[gray]HP[/]";
                    var task = x.AddTask(status);
                    float percentage = (float)m.CurrentHP * 100 / m.MaxHP;
                    task.Increment(percentage);
                });
            Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
            AnsiConsole.MarkupLine(m.IsAlive ? $"[red]{m.CurrentHP}/{m.MaxHP}[/]\n" : $"[gray]{m.CurrentHP}/{m.MaxHP}[/]\n");
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
            AnsiConsole.MarkupLine(m.IsAlive ? $"{new string('-', partician_Length)}\n" : $"[gray]{new string('-', partician_Length)}[/]\n");
        }

        void Result(Player player, List<Monster> monsters)
        {
            int killCount = 0;
            string panelHeader = "";

            // 전투 승리/패배에 따라
            // 처치한 몬스터에 따라 얻은 골드, 경험치 계산
            if (player.hp > 0)
            {
                int gold = 0,
                    increasePerLoop_gold = 0,
                    exp = 0,
                    increasePerLoop_exp = 0,
                    resultLoopCount = 0;

                panelHeader += "[green]Victory[/]";
                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");

                // 모든 몬스터를 잡았기에 몬스터 수 전체를 잡은 수에 추가
                killCount = monsters.Count;
                // 전투 승리시에만 처치한 몬스터들로부터 골드, 경험치 획득량 계산
                for (int i = 0; i < monsters.Count; i++)
                {
                    gold += monsters[i].DropGold;
                    exp += monsters[i].DropExp;
                }

                // 경험치,골드 획득!
                player.gold += gold;
                player.AddExp(exp);

                // 결과창 보여주는 루프 횟수, 루프 당 값 상승량
                resultLoopCount = 10;
                increasePerLoop_gold = gold / resultLoopCount;
                increasePerLoop_exp = exp / resultLoopCount;
                // 보여주기 위한 초기화
                gold = exp = 0;

                // 결과창의 숫자가 점점 증가하는 것을 보여주기 위한 루프
                for (int i = 0; i < resultLoopCount; i++)
                {
                    // 패널 안에 들어갈 내용
                    string inPanel =
                    $"[yellow bold]{player.playerName}[/] ({player.playerClass}) Lv. {player.level}\n" +
                    $"HP {player.hp} / {player.maxHp}\n" +
                    $"MP [blue]{++player.mana}[/] / {player.maxMp}\n\n" +
                    $"<< 보상 >>\n" +
                    $"골  드: +[yellow]{gold}[/] G\n" +
                    $"경험치: +[yellow]{exp}[/] Exp";

                    // 전투 승리 후 플레이어 마나 자연적으로 10 회복 // 루프당 1씩 회복되는 모습
                    // 드랍 아이템... 없다??? 없으면 아이템은 빼기

                    Console.Clear();
                    Console.WriteLine("Battle!! = Result\n");
                    var panel = new Panel(inPanel); // 패널 생성 및 안에 들어갈 내용 
                                                    // 테두리 스타일(Rounded, Square, Ascii, None 등) Double, Heavy는 제대로 출력이 안되는 듯함
                    panel.Border = BoxBorder.Rounded;
                    // 패널 상단 중앙에 제목 적기(왼쪽, 오른쪽으로도 변경 가능)
                    panel.Header = new PanelHeader(panelHeader, Justify.Center);
                    // 패널 출력
                    AnsiConsole.Write(panel);
                    
                    // 다음 루프를 위한 더하기
                    gold += increasePerLoop_gold;
                    exp += increasePerLoop_exp;

                    // 숫자가 변하는 것을 보여주기 위한 딜레이
                    Thread.Sleep(100);
                }
            }
            else
            {
                // 패널 헤더
                panelHeader += "[red]You Lose[/]";
                // 패널 안에 들어갈 내용
                string inPanel =
                $"Lv.{player.level} {player.playerName}\n" +
                $"HP {player.hp}/{player.maxHp}\n" +
                $"MP {player.mana}/{player.maxMp}";

                Console.Clear();
                Console.WriteLine("Battle!! = Result\n");
                var panel = new Panel(inPanel); // 패널 생성 및 안에 들어갈 내용 
                                                // 테두리 스타일(Rounded, Square, Ascii, None 등) Double, Heavy는 제대로 출력이 안되는 듯함
                panel.Border = BoxBorder.Rounded;
                // 패널 상단 중앙에 제목 적기(왼쪽, 오른쪽으로도 변경 가능)
                panel.Header = new PanelHeader(panelHeader, Justify.Center);
                // 패널 출력
                AnsiConsole.Write(panel);

                // 처치한 몬스터 수만 카운트
                if (Program.quest.Get_questData().ContainsKey(0))
                {
                    for (int i = 0; i < monsters.Count; i++)
                    {
                        if (!monsters[i].IsAlive)
                            killCount++;
                    }
                }
            }

            // 퀘스트에 잡은 몬스터 수를 카운트
            Program.quest.QuestRenewal(0, killCount);


            Console.WriteLine("\n0. 다음\n>>");
            Console.ReadLine();
        }
        private void BasicAttack(Player player, List<Monster> monsters)
        {

            Random rand = new Random();//플레이어에게 크리티컬 확률이없어 일시적으로 추가
            int crit = 15;// 22
            int eva = 10;
            crit = rand.Next(1, 101);
            eva = rand.Next(1, 101);

            Console.Clear();
            Console.WriteLine("=== 공격할 대상을 선택하세요 ===\n");

            // 몬스터 리스트 출력
            for (int i = 0; i < monsters.Count; i++)
            {
                var m = monsters[i];
                string status = m.CurrentHP <= 0 ? "Dead" : $"HP {m.CurrentHP}";
                Console.WriteLine($"{i + 1}. Lv.{m.Level} {m.Name} - {status}");
            }

            Console.WriteLine("\n0. 취소");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "0") return;

            if (!int.TryParse(input, out int index) || index < 1 || index > monsters.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                return;
            }

            Monster target = monsters[index - 1];

            if (!target.IsAlive)
            {
                Console.WriteLine("이미 쓰러진 몬스터입니다.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Battle!!\n");

            //int damage = (int)(player.totalPower * (100f / (100 + target.Defense)));
            //damage = Math.Max(3, damage); //-> 데미지가 너무약하면 아래 코드 주석처리하고 이코드 풀어서 사용하세요
            int damage = Math.Max(1, (int)(player.totalPower - target.Defense));

            // 공격 처리
            if (eva <= 10)
            {
                Console.WriteLine($"{player.playerName} 의 공격!");
                Console.WriteLine($"Lv.{target.Level} {target.Name} 을(를) 공격했지만 아무일도 일어나지 않았습니다.");
            }
            else
            {
                bool isCritical = crit <= 15;
                int finalDamage = isCritical ? (int)(damage * 1.6) : damage;
                target.Hit(finalDamage);

                Console.WriteLine($"{player.playerName} 의 공격!");
                Console.WriteLine(isCritical ? $"Lv.{target.Level} {target.Name} 을(를) 맞혔습니다. [데미지 : {finalDamage}] - 치명타 공격!!"
                    : $"Lv.{target.Level} {target.Name} 을(를) 맞혔습니다. [데미지 : {finalDamage}]");
            }


            //여기에 데미지 처리
            if (!target.IsAlive)
                Console.WriteLine($"\nLv.{target.Level} {target.Name}\nHP {target.MaxHP} -> Dead");
            else
                Console.WriteLine($"\nLv.{target.Level} {target.Name}\nHP {target.CurrentHP}");

            Console.WriteLine("\n0. 다음\n>>");
            Console.ReadLine();

            // 살아있는 몬스터들의 반격
            Console.Clear();
            MonsterCounterAttack(player, monsters);
        }

        private void SkillAttack(Player player, List<Monster> monsters)
        {
            Console.Clear();

            // 1. 직업 스킬 목록 뽑기
            List<Skill> jobSkills = new List<Skill>();
            foreach (var skill in Skill.SkillList.Values)
            {
                if (skill.Job == player.playerClass)
                {
                    jobSkills.Add(skill);
                }
            }

            if (jobSkills.Count == 0)
            {
                Console.WriteLine("사용 가능한 스킬이 없습니다.");
                Console.ReadKey();
                return;
            }

            // 2. 스킬 선택
            Console.WriteLine($"{player.playerClass}의 사용 가능한 스킬 목록:");
            for (int i = 0; i < jobSkills.Count; i++)
            {
                var s = jobSkills[i];
                Console.WriteLine($"{i + 1}. {s.Name} (배수: {s.PowerMultiplier}, 마나 소모: {s.ManaCost})");
            }

            Console.Write("\n사용할 스킬 번호를 선택하세요 >> ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int selected) || selected < 1 || selected > jobSkills.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                return;
            }

            Skill chosenSkill = jobSkills[selected - 1];

            // 3. 마나 체크
            if (player.mana < chosenSkill.ManaCost)
            {
                Console.WriteLine("마나가 부족합니다.");
                Console.ReadKey();
                return;
            }

            // 4. 마나 차감
            player.mana -= chosenSkill.ManaCost;

            // 5. 전체 몬스터 공격
            Console.Clear();
            Console.WriteLine($"{player.playerName}의 [{chosenSkill.Name}] 발동!");
            Thread.Sleep(1000);

            int skillDamage = player.SkillPower(chosenSkill);
            foreach (var monster in monsters)
            {
                if (monster.CurrentHP <= 0)
                {
                    Console.WriteLine($"→ {monster.Name} 은(는) 이미 쓰러져 있습니다.");
                    continue;
                }

                int damage = Math.Max(1, skillDamage - monster.Defense);
                monster.CurrentHP -= damage;

                Console.WriteLine($"→ {monster.Name} 에게 {damage} 데미지!");
                Thread.Sleep(300);
            }

            Console.WriteLine("\n스킬 공격 종료!");
            Console.WriteLine("아무키나 입력해주세요");
            Console.WriteLine(">>");
            Console.ReadKey();
            Console.Clear();
            MonsterCounterAttack(player, monsters);


        }

        private void MonsterCounterAttack(Player player, List<Monster> monsters)
        {
            // 반격할 몬스터가 없는데 반격하는 경우 발생
            if (Monsters.All(m => m.IsAlive == false))
                return;

            Console.WriteLine("몬스터가 반격합니다!!\n");
            foreach (var m in monsters)
            {
                if (m.CurrentHP > 0)
                {
                    int retaliation = Math.Max(1, m.Attack - player.totalDefense);
                    player.hp -= retaliation;

                    Console.WriteLine($"Lv.{m.Level} {m.Name} 의 공격!");
                    Console.WriteLine($"{player.playerName} 을(를) 맞혔습니다. [데미지 : {retaliation}]");
                    Console.WriteLine($"\nLv.{player.level} {player.playerName}\nHP {player.hp + retaliation} → {player.hp}\n");

                    Thread.Sleep(500);



                    if (player.hp <= 0)
                        break;
                }
            }
            Console.Write("넘어가시려면 아무키나 눌러주세요");
            Console.ReadKey();
        }
    }
}
