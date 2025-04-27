using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NAudio.Codecs;
using Spectre.Console;
using static TextRpg.GameConstance;

namespace TextRpg
{
    class Dungeon
    {
        Random rand = new Random();
        private List<Stage> _stages;

        public void EnterDungeonMenu()
        {
            while (true)
            {

                Console.Clear();
                AnsiConsole.MarkupLine("**던전입장**");
                AnsiConsole.MarkupLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                string[] menus_Seletable = new string[] { "1. 상태 보기", "2. 전투 시작\n", "0. 나가기" };

                var menu = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                           .Title("무엇을 하시겠습니까?")
                           .PageSize(3) // 항목 수(최소 3)
                           .AddChoices(menus_Seletable)
                           .WrapAround()); // 리스트 순환 >> 맨 위 항목에서 위 방향키를 누르면 제일 아래 항목으로. 역도 성립

                // 선택한 메뉴가 몇번째인가?
                int index = 0;
                foreach (var menu_Seletable in menus_Seletable)
                {
                    if (menu_Seletable == menu)
                        break;
                    else
                        index++;
                }

                // 선택지 선택에 따른 동작
                if (index == 0)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine(Program.player.PrintPlayer());
                    AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("나가기\n"));
                }
                else if (index == 1)
                {
                    SoundManager.Instance.StopMusic();
                    EnterDungeon();
                }
                else
                {
                    return;
                }
            }
        }
        private void EnterDungeon()
        {
            SoundManager.Instance.StartDungeonMusic();
            Console.Clear();
            _stages = new List<Stage>();
            int maxStage = GameConstance.Dungeon.MAX_STAGE;

            // try
            // {
                Console.Clear();
                AnsiConsole.MarkupLine("Battle");

                for (int i = 1; i <= maxStage; i++)
                {
                    _stages.Add(new Stage($"{i}단계", MonsterFactory.Create(i)));
                    // stages.Add(new Stage("1단계", new Dictionary<string, int> { { "1", 3 } }));
                    // stages.Add(new Stage("2단계", new Dictionary<string, int> { { "1", 2 }, { "2", 1 } }));
                    // stages.Add(new Stage("3단계", new Dictionary<string, int> { { "2", 2 }, { "3", 1 } }));
                    // stages.Add(new Stage("4단계", new Dictionary<string, int> { { "3", 2 }, { "4", 1 } }));
                    // stages.Add(new Stage("5단계", new Dictionary<string, int> { { "4", 2 }, { "5", 1 } }));
                    // stages.Add(new Stage("6단계", new Dictionary<string, int> { { "5", 2 }, { "6", 1 } }));
                    // stages.Add(new Stage("7단계", new Dictionary<string, int> { { "6", 2 }, { "7", 1 } }));
                    // stages.Add(new Stage("8단계", new Dictionary<string, int> { { "7", 2 }, { "8", 1 } }));
                    // stages.Add(new Stage("9단계", new Dictionary<string, int> { { "8", 2 }, { "9", 1 } }));
                    // stages.Add(new Stage("10단계", new Dictionary<string, int> { { "8", 1 }, { "9", 1 }, { "10", 1 } }));
                }

                foreach (Stage stage in _stages)
                {
                    stage.Start();
                    if (Program.player.hp <= 0)
                    {
                        AnsiConsole.MarkupLine("플레이어가 사망했습니다. 던전에서 퇴장합니다...");
                        Thread.Sleep(2000);
                        SoundManager.Instance.StopMusic();
                        SoundManager.Instance.StartMainMusic();
                        break;
                    }

                    if (!stage.Cleared)
                    {
                        AnsiConsole.MarkupLine("스테이지 클리어에 실패했습니다...");
                        Thread.Sleep(2000);
                        SoundManager.Instance.StopMusic();
                        SoundManager.Instance.StartMainMusic();
                        break;
                    }

                    // 다음 스테이지로 진행 여부
                    Console.Clear();
                    AnsiConsole.MarkupLine($"현재까지 [#6cf540]{stage.Name}[/] 스테이지 클리어!");
                    string[] menus_Seletable = new string[] { "1. 다음 스테이지 도전", "0. 마을로 돌아가기" };

                    var menu = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                               .Title("무엇을 하시겠습니까?")
                               .PageSize(3) // 항목 수(최소 3)
                               .AddChoices(menus_Seletable)
                               .WrapAround()); // 리스트 순환 >> 맨 위 항목에서 위 방향키를 누르면 제일 아래 항목으로. 역도 성립

                    // 선택한 메뉴가 몇번째인가?
                    int index = 0;
                    foreach (var menu_Seletable in menus_Seletable)
                    {
                        if (menu_Seletable == menu)
                            break;
                        else
                            index++;
                    }

                    // 선택지 선택에 따른 동작
                    if (index == 1)
                    {
                        AnsiConsole.MarkupLine("던전을 떠납니다...");
                        Thread.Sleep(1000);
                        SoundManager.Instance.StopMusic();
                        SoundManager.Instance.StartMainMusic();
                        break;
                    }
                }

            // }
            // catch (Exception ex)
            // {
            //     AnsiConsole.MarkupLine($"[[에러 발생]] {ex.Message}");
            //     Console.ReadKey();
            // }

        }
    }
    public class Stage
    {
        public string Name { get; private set; }//던전이름
        public List<Monster> Monsters { get; private set; }//몬스터 리스트
        private bool _cleared = false;

        public bool Cleared { 
            get { return _cleared; }
         }

        
        int barWidth = 20, // 진행바 길이
            partician_Length = 15, // 유닛 간 나누는 줄 길이
            selectedIndex = 0, // 타겟 선택할 때 사용하는 인덱스
            resultLoopCount = 5, // 결과창 보여주는 루프 횟수
            loopDelay = 150; // 변화하는 것을 보여주기 위한 루프 사이의 딜레이

        // private static bool MonstersLoaded = false;//

        public Stage(string name, List<Monster> monsters)
        {
            // if (!MonstersLoaded)
            // {
            //     string filePath = @"..\..\..\Monsters.csv";
            //     //상대경로 수정필요
            //     MonsterFactory.LoadMonsters(filePath);
            //     MonstersLoaded = true;
            // }

            Name = name;
            Monsters = monsters;

            // 진행바 길이에 따라 변동
            partician_Length += barWidth;
        }

        internal void Start()
        {
            AnsiConsole.MarkupLine($"=== {Name} 스테이지 시작 ===");

            foreach (Monster monster in Monsters)
            {
                AnsiConsole.MarkupLine(monster.ToString());

            }

            Battle();


            AnsiConsole.MarkupLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();
        }


        private void Battle()
        {
            // 몬스터가 다 죽었을 때는 루프를 빠져나가서 보상을 받을 수 있도록 변경
            while (true)
            {
                if (Monsters.All(m => m.CurrentHP <= 0))
                {
                    // 모든 몬스터가 죽었으면 전투 종료하고 결과 화면으로 바로 넘어감
                    break;
                }

                Console.Clear();
                AnsiConsole.MarkupLine("Battle!!\n");

                // 몬스터 목록 출력
                for (int i = 0; i < Monsters.Count; i++)
                {
                    PrintEnemy(Monsters[i], i, false);
                }

                // 플레이어 출력(경험치 바는 제외)
                PrintPlayer(false);

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
                    BasicAttack();
                    continue;
                }
                else if (index == 1)
                {
                    SkillAttack();
                    continue;
                }
                else if (index == 2)
                    return; // 도망
            }

            // 던전 클리어 결과창
            Result();
        }

        void PrintPlayer(bool isWithExpBar)
        {
            AnsiConsole.MarkupLine($"[#6cf540]{new string('-', partician_Length)}[/]");
            // 플레이어
            AnsiConsole.MarkupLine($"[yellow bold]{Program.player.playerName}[/] ({Program.player.playerClass}) Lv. {Program.player.level}");
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
                    var task = x.AddTask("[red]HP [/]");
                    float percentage = (float)Program.player.hp * 100 / Program.player.maxHp;
                    task.Increment(percentage);
                });
            Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
            AnsiConsole.MarkupLine($"[red]{Program.player.hp}/{Program.player.maxHp}[/]" + new string(' ', (Program.player.maxHp.ToString().Length)));

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
                    var task = x.AddTask("[blue]MP [/]");
                    float percentage = (float)Program.player.mana * 100 / Program.player.maxMp;
                    task.Increment(percentage);
                });
            // 진행바 오른쪽에 커스텀 텍스트를 표시할 방법이 없음.. 그럼 커서 옮기고 써주면 되는 거 아닌가?
            Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
            AnsiConsole.MarkupLine($"[blue]{Program.player.mana}/{Program.player.maxMp}[/]" + new string(' ', (Program.player.maxMp.ToString().Length)));

            // Exp 바도 함께 출현
            if(isWithExpBar)
            {
                // 경험치 바 (초록)
                AnsiConsole
                    .Progress()
                    .Columns(
                    // Column의 순서 변경 및 바 색상 변경
                    new TaskDescriptionColumn(), // 설명
                    new ProgressBarColumn
                    {
                        Width = barWidth, // 진행바 길이
                        CompletedStyle = new Style(foreground: Color.FromHex("#6cf540")), // 채워진 부분 색상
                        RemainingStyle = new Style(foreground: Color.FromHex("#62911A")), // 채워지지 않은 부분 색상
                        FinishedStyle = new Style(foreground: Color.FromHex("#6cf540")), // 100% 일 때의 색상
                    })
                    .Start(x =>
                    {
                        var task = x.AddTask("[#6cf540]EXP[/]");
                        double percentage = Program.player.exp * 100 / Program.player.maxExp;
                        task.Increment(percentage);
                    });
                // 진행바 오른쪽에 커스텀 텍스트를 표시할 방법이 없음.. 그럼 커서 옮기고 써주면 되는 거 아닌가?
                Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
                AnsiConsole.MarkupLine($"[#6cf540]{Program.player.exp}/{Program.player.maxExp}[/]");
            }
            // 아래 구분선
            AnsiConsole.MarkupLine($"[#6cf540]{new string('-', partician_Length)}[/]\n");
        }


        void PrintEnemy(Monster monster, int i, bool isTarget)
        {
            var m = monster;

            // 타겟이라면 테두리를 다른 색으로 표시
            if(isTarget)
                AnsiConsole.MarkupLine($"[blue]{new string('-', partician_Length)}[/]");
            else
                AnsiConsole.MarkupLine(m.IsAlive ? $"{new string('-', partician_Length)}" : $"[gray]{new string('-', partician_Length)}[/]");
            // 적 정보
            string status = "";
            if (isTarget)
                status = $"[blue]{i + 1}. {m.Name} Lv.{m.Level}[/]";
            else
                status = m.IsAlive ? $"{i + 1}. {m.Name} Lv.{m.Level}" : $"[gray]{i + 1}. Lv.{m.Name} {m.Level} (Dead)[/]";
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
                    string status = m.IsAlive ? $"[red]HP [/]" : "[gray]HP [/]";
                    var task = x.AddTask(status);
                    double percentage = m.CurrentHP * 100 / m.MaxHP;
                    task.Increment(percentage);
                });
            Console.SetCursorPosition(barWidth + 5, Console.GetCursorPosition().Top - 2);
            AnsiConsole.MarkupLine(m.IsAlive ? $"[red]{(int)m.CurrentHP}/{m.MaxHP}[/]" : $"[gray]{(int)m.CurrentHP}/{m.MaxHP}[/]" + new string(' ', (m.MaxHP.ToString().Length))); // 현재 HP의 자릿수가 바뀌었을 때 자릿수가 줄어들기에 제대로 덮어쓰지 못하는 경우 발생. 공백 써주기
            // 타겟이라면 테두리를 다른 색으로 표시
            if (isTarget)
                AnsiConsole.MarkupLine($"[blue]{new string('-', partician_Length)}[/]");
            else
                AnsiConsole.MarkupLine(m.IsAlive ? $"{new string('-', partician_Length)}" : $"[gray]{new string('-', partician_Length)}[/]");
        }


        void Result()
        {
            int killCount = 0;
            string resultTitle = "";

            // 전투 승리/패배에 따라
            // 처치한 몬스터에 따라 얻은 골드, 경험치 계산
            if (Program.player.hp > 0)
            {
                _cleared = true;

                int gold = 0,
                    exp = 0;

                resultTitle += "[bold #6cf540]Victory[/]";
                // 모든 몬스터를 잡았기에 몬스터 수 전체를 잡은 수에 추가
                killCount = Monsters.Count;
                // 전투 승리시에만 처치한 몬스터들로부터 골드, 경험치 획득량 계산
                for (int i = 0; i < Monsters.Count; i++)
                {
                    gold += Monsters[i].DropGold;
                    exp += Monsters[i].DropExp;
                }

                // 골드 획득!
                Program.player.gold += gold;
                double expIncreasePerLoop = exp / resultLoopCount;

                // 결과창의 숫자가 점점 증가하는 것을 보여주기 위한 루프
                for (int i = 0; i < resultLoopCount; i++)
                {
                    // 이번 루프에서 표시를 위한 스텟 변화
                    Program.player.mana = Math.Clamp(Program.player.mana + 1, 0, Program.player.maxMp); // 1씩 10번. 총 10 마나 회복
                    Program.player.AddExp(expIncreasePerLoop); // 획득 경험치/loopCount 만큼씩 획득

                    // 패널 안에 들어갈 내용
                    string inPanel =
                    $"골  드: +[yellow]{gold * i / resultLoopCount}[/] G\n" +
                    $"경험치: +[yellow]{expIncreasePerLoop * i}[/] Exp";
                    

                    // 전투 승리 후 플레이어 마나 자연적으로 10 회복 // 루프당 1씩 회복되는 모습
                    // 드랍 아이템... 없다??? 없으면 아이템은 빼기

                    // 바꾼 데이터를 표시
                    Console.Clear();
                    AnsiConsole.MarkupLine("Battle!! - Result\n" + resultTitle);
                    // 플레이어 출력
                    PrintPlayer(true);
                    var panel = new Panel(inPanel); // 패널 생성 및 안에 들어갈 내용 
                                                    // 테두리 스타일(Rounded, Square, Ascii, None 등) Double, Heavy는 제대로 출력이 안되는 듯함
                    panel.Border = BoxBorder.Rounded;
                    // 패널 상단 중앙에 제목 적기(왼쪽, 오른쪽으로도 변경 가능)
                    panel.Header = new PanelHeader("<< 보상 >>", Justify.Center);
                    // 패널 출력
                    AnsiConsole.Write(panel);
                    
                    // 숫자가 변하는 것을 보여주기 위한 딜레이
                    Thread.Sleep(loopDelay); // 딜레이가 너무 짧으면 깜빡이는 현상 심함.. 그렇다고 길게 하면 결과창이 루즈함.. 적당한 값으로 쓰기
                }
            }
            else
            {
                resultTitle += "[bold red]You Lose[/]\n\n던전 공략에 실패하였습니다.";
                Console.Clear();
                AnsiConsole.MarkupLine("Battle!! = Result\n" + resultTitle);
                // 플레이어 출력
                PrintPlayer(true);

                // 처치한 몬스터 수만 카운트
                if (Program.quest.Get_questData().ContainsKey(0))
                {
                    for (int i = 0; i < Monsters.Count; i++)
                    {
                        if (!Monsters[i].IsAlive)
                            killCount++;
                    }
                }
            }

            // 퀘스트에 잡은 몬스터 수를 카운트
            Program.quest.QuestRenewal(0, killCount);
        }

        // 플레이어의 공격력에 오차 +,- 10%를 주어 몬스터 방어력과 계산 후 최종 대미지를 산출하여 리턴
        int damageError(double damageInit, int Monsterdef)
        {
            Random rand = new Random();

            // 공격 시 오차 +,- 10% >> 공격력에 0.9~1.1 곱하기 >> 0.9~1.1 사이가 나오게끔 랜덤
            // rand.NextDouble() = 0~1 사이 값 랜덤
            // 최소값 0.9에 0~0,2 사이 랜덤값을 더하면 0.9~1.1 랜덤
            double error = 0.9 + rand.NextDouble() * 0.2;
            // 오차로 인해 발생한 소숫점은 올림 처리
            int atk_withError = (int)(Math.Ceiling(error * damageInit));
            // 방어력이 공격력보다 높아도 적어도 1의 대미지가 들어가게끔 조치
            int damage = Math.Max(1, (atk_withError - Monsterdef));

            return damage;
        }

        void SelectTarget()
        {
            // 타겟 선택 루프
            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine("=== 공격할 대상을 선택하세요 ===\n");

                // 몬스터 리스트 출력
                for (int i = 0; i < Monsters.Count; i++)
                {
                    var m = Monsters[i];
                    // 해당 몬스터가 지금 타겟이라면
                    if (selectedIndex == i)
                    {
                        // 살아있다면, 타겟 표시
                        if (m.IsAlive)
                            PrintEnemy(m, i, true);
                        // 죽었다면, 표시(내부적으로 죽음 표시 처리)하고 다음 타겟으로
                        else
                        {
                            PrintEnemy(m, i, false);
                            selectedIndex++;
                        }
                    }
                    // 타겟이 아니라면 표시
                    else
                        PrintEnemy(m, i, false);
                }
                // 플레이어 출력
                PrintPlayer(false);
                if (selectedIndex == Monsters.Count)
                    AnsiConsole.Markup("[blue]취소[/]");
                else
                    AnsiConsole.Markup("취소");

                // 선택지
                ConsoleKey enter = Console.ReadKey().Key;

                if (enter == ConsoleKey.Enter || enter == ConsoleKey.Spacebar)
                {
                    if (selectedIndex == Monsters.Count)
                        return;
                    else
                        break; // selectedIndex 값을 가지고 타겟 선택 루프를 빠져나가도록
                }
                else if (enter == ConsoleKey.UpArrow)
                {
                    while (true)
                    {
                        // 한칸 위를 가리키도록
                        --selectedIndex;
                        // 0번째(첫 몬스터)에서 방향키를 위로 눌렀다면
                        if (selectedIndex < 0)
                        {
                            // 취소 항목 가리키고 루프 종료
                            selectedIndex = Monsters.Count;
                            break;
                        }
                        // 한칸 위의 몬스터가 살아있다면 루프 종료
                        else if (Monsters[selectedIndex].IsAlive)
                            break;
                        // 죽은 몬스터를 가르켰다면 루프 다시
                    }
                }
                else if (enter == ConsoleKey.DownArrow)
                {
                    while (true)
                    {
                        // 한칸 아래를 가리키도록
                        ++selectedIndex;
                        // 취소 항목에서 아래 방향키를 눌렀다면
                        if (selectedIndex > Monsters.Count)
                            // 첫 항목으로
                            selectedIndex = 0;

                        // Monsters.Count 는 취소 버튼 인덱스라서 따로 처리. 취소 버튼 가리키고 루프 종료
                        if (selectedIndex == Monsters.Count)
                            break;
                        // 한칸 아래의 몬스터가 살아있다면 루프 종료
                        else if (Monsters[selectedIndex].IsAlive)
                            break;
                        // 죽은 몬스터를 가리켰다면 다시 루프
                    }
                }
            }
        }

        // 기본 공격
        private void BasicAttack()
        {
            // 타겟 초기화
            selectedIndex = 0;

            // 타겟 선택
            SelectTarget();

            // 취소를 선택했다면, 공격을 수행하지 않고 리턴
            if (selectedIndex == Monsters.Count)
                return;

            // 타겟 지정
            Monster target = Monsters[selectedIndex];

            // 공격 수행
            // 플레이어의 공격력에 오차 +,- 10%를 주어 몬스터 방어력과 계산 후 대미지 산출
            int damage = damageError(Program.player.totalPower, target.Defense),
                finalDamage = 0;
            // 회피, 치명 랜덤
            Random rand = new Random();
            int crit = rand.Next(1, 101);
            int eva = rand.Next(1, 101);
            string panelHeader = $"< {Program.player.playerName} 의 공격! >"; // 전투 패널 헤더
            string message = ""; // 전투 패널에 들어갈 메시지
            bool isEvade = false; // 회피 여부
            // 기억할 커서 위치
            int left_target = 0,
                top_target = 0,
                left_end = 0,
                top_end = 0;
            // 회피
            if (eva <= 10)
            {
                message += $"Lv.{target.Level} {target.Name} 은 공격을 피했습니다.";
                isEvade = true;
            }
            // 크리티컬 판정 및 대미지 처리
            else
            {
                bool isCritical = crit <= Program.player.criticalChance;
                finalDamage = isCritical ? (int)(damage * 1.6) : damage;
                //target.Hit(finalDamage);

                message += isCritical 
                    ? $"Lv.{target.Level} {target.Name} 을(를) 공격했습니다. [[데미지 : [red]{finalDamage}[/]]] - [red]치명타 공격!![/]"
                    : $"Lv.{target.Level} {target.Name} 을(를) 공격했습니다. [[데미지 : [yellow]{finalDamage}[/]]]";
            }

            Console.Clear();
            AnsiConsole.MarkupLine("Battle!!\n");
            // 몬스터 출력
            for (int j = 0; j < Monsters.Count; j++)
            {
                // 타겟 몬스터를 표현하기 시작하는 커서 위치 기억
                if (j == selectedIndex)
                {
                    left_target = Console.GetCursorPosition().Left;
                    top_target = Console.GetCursorPosition().Top;
                }
                PrintEnemy(Monsters[j], j, false);
            }
            // 플레이어 출력
            PrintPlayer(false);
            // 공격 메세지 출력
            var panel = new Panel(message);
            panel.Border = BoxBorder.Rounded;
            // 패널 상단 중앙에 제목 적기(왼쪽, 오른쪽으로도 변경 가능)
            panel.Header = new PanelHeader(panelHeader, Justify.Center);
            // 패널 출력
            AnsiConsole.Write(panel);
            // 모든 메세지의 출력이 끝난 위치를 기억
            left_end = Console.GetCursorPosition().Left;
            top_end = Console.GetCursorPosition().Top;

            // 회피하지 않았다면
            if (!isEvade)
            {
                // 화면에 공격 결과 표시
                for (int i = 0; i < resultLoopCount; i++)
                {
                    // 피해량을 loop 횟수만큼 나누어서 쭈우욱 HP바가 감소하는 느낌
                    target.Hit(finalDamage / resultLoopCount);
                    // 타겟 몬스터의 상태를 써주는 위치로 커서 이동
                    Console.SetCursorPosition(left_target, top_target);
                    // 타겟 몬스터의 HP 감소를 덮어써줌
                    PrintEnemy(Monsters[selectedIndex], selectedIndex, false);

                    // 사망 시 메세지 출현하고 루프 종료
                    if (!target.IsAlive)
                    {
                        // 출현한 메세지 뒤로 커서 이동
                        Console.SetCursorPosition(left_end, top_end);
                        // 적 사망 메세지 출현
                        AnsiConsole.MarkupLine($"{selectedIndex+1}. {target.Name} Lv.{target.Level} 은(는) 쓰러졌습니다.");
                        // 커서 위치 경신
                        left_end = Console.GetCursorPosition().Left;
                        top_end = Console.GetCursorPosition().Top;
                        break;
                    }

                    // 체력이 깎이는 과정을 보여주기 위한 딜레이
                    Thread.Sleep(loopDelay);
                }
            }

            // 출현한 메세지 뒤로 커서 이동
            Console.SetCursorPosition(left_end, top_end);
            // 몬스터 모두 처치했는지 확인
            if (Monsters.All(m => m.IsAlive == false))
            {
                AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("[#6cf540]모든 몬스터를 처치했습니다![/]\n"));
                return; // 함수 종료
            }
            else
                AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("다음\n"));

            Console.Clear();
            MonsterCounterAttack();
        }

        // 스킬 선택 UI 출력
        void PrintSkill(Skill skill, bool isSelected)
        {
            Panel panel = null;
            string info = $"{(skill.Type == SkillType.Single ? "적 하나" : "적 전체")}에 공격력의 {skill.PowerMultiplier}만큼 피해를 줍니다.\n마나 소모: {skill.ManaCost})";
            if (isSelected)
            {
                panel = new Panel($"[blue]{info}[/]");
                panel.Border = BoxBorder.Rounded;
                panel.Header = new PanelHeader($"[blue]< {skill.Name} >[/]", Justify.Center);
                panel.BorderColor(Color.Blue);
            }
            else
            {
                panel = new Panel(info); // 패널 생성 및 안에 들어갈 내용
                panel.Border = BoxBorder.Rounded;
                panel.Header = new PanelHeader($"< {skill.Name} >", Justify.Center);
            }
            // 패널 출력
            AnsiConsole.Write(panel);
        }

        private void SkillAttack()
        {
            // 타겟 초기화
            selectedIndex = 0;
            Console.Clear();
            // 1. 직업 스킬 목록 뽑기
            List<Skill> jobSkills = SkillFactory.GetSkillsByJob(Program.player.playerClass);
            
            // 스킬이 없다면
            if (jobSkills.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]사용 가능한 스킬이 없습니다.[/]");
                Console.ReadKey();
                return;
            }

            // 2. 스킬 선택
            AnsiConsole.MarkupLine($"{Program.player.playerClass}의 사용 가능한 스킬 목록:");
            for (int i = 0; i < jobSkills.Count; i++)
            {
                var s = jobSkills[i];
                AnsiConsole.MarkupLine($"{i + 1}. {s.Name} (배수: {s.PowerMultiplier}, 마나 소모: {s.ManaCost})");
            }

            // 이런 방식으로 번호 입력 구현.. 했으나 선택지로 변경
            //Console.Write("\n사용할 스킬 번호를 선택하세요 >> ");
            //string input = Console.ReadLine();
            //if (!int.TryParse(input, out int selected) || selected < 1 || selected > jobSkills.Count)
            //{
            //    AnsiConsole.MarkupLine("잘못된 입력입니다.");
            //    Console.ReadKey();
            //    return;
            //}

            Skill chosenSkill = jobSkills[selected - 1];

            // 3. 마나 체크
            if (Program.player.mana < chosenSkill.ManaCost)
            {
                AnsiConsole.MarkupLine("마나가 부족합니다.");
                Console.ReadKey();
                return;
            }

            // 4. 마나 차감
            Program.player.mana -= chosenSkill.ManaCost;

            // 5. 전체 몬스터 공격
            Console.Clear();
            AnsiConsole.MarkupLine($"{Program.player.playerName}의 [[{chosenSkill.Name}]] 발동!");
            Thread.Sleep(1000);

            int skillDamage = Program.player.SkillPower(chosenSkill);
            foreach (var monster in Monsters)
            {
                if (monster.CurrentHP <= 0)
                {
                    AnsiConsole.MarkupLine($"→ {monster.Name} 은(는) 이미 쓰러져 있습니다.");
                    continue;
                }

                int damage = Math.Max(1, skillDamage - monster.Defense);
                monster.CurrentHP -= damage;

                AnsiConsole.MarkupLine($"→ {monster.Name} 에게 {damage} 데미지!");
                Thread.Sleep(300);
            }

            AnsiConsole.MarkupLine("\n스킬 공격 종료!");
            AnsiConsole.MarkupLine("아무키나 입력해주세요");
            AnsiConsole.MarkupLine(">>");
            Console.ReadKey();
            Console.Clear();

            if (Monsters.All(m => m.CurrentHP <= 0))
            {
                AnsiConsole.MarkupLine("모든 몬스터를 처치했습니다!");
                Console.ReadKey();
                return; // 함수 종료
            }

            
            MonsterCounterAttack();
        }

        // 몬스터의 반격 페이즈
        private void MonsterCounterAttack()
        {
            // 반격할 몬스터가 없는데 반격하는 경우 방지
            if (Monsters.All(m => m.IsAlive == false))
                return;

            Console.Clear();
            AnsiConsole.MarkupLine("[red]몬스터가 반격합니다!![/]\n");
            // 몬스터들 출력
            for (int i = 0; i < Monsters.Count; i++)
            {
                PrintEnemy(Monsters[i], i, false);
            }

            // 플레이어를 써주기 시작할 때의 커서 좌표
            int left = Console.GetCursorPosition().Left,
                top = Console.GetCursorPosition().Top;

            List<string>[] counterMessages = new List<string>[2] { new List<string>(), new List<string>()};
            for(int j = 0; j < Monsters.Count; j++)
            {
                var m = Monsters[j];
                if (m.CurrentHP > 0)
                {
                    // 대미지 계산
                    int retaliation = Math.Max(1, m.Attack - Program.player.totalDefense);
                    Program.player.hp -= retaliation;

                    counterMessages[0].Add($"< {j+1}. {m.Name} Lv.{m.Level} 의 공격! >");
                    counterMessages[1].Add($"{Program.player.playerName} 을(를) 공격했습니다. [[데미지 : [red]{retaliation}[/]]]\n[yellow bold]{Program.player.playerName}[/] ({Program.player.playerClass}) Lv. {Program.player.level}\nHP [#6cf540]{Program.player.hp + retaliation}[/] → [#6cf540]{Program.player.hp}[/]");

                    Console.SetCursorPosition(left, top);
                    PrintPlayer(false);
                    // 몬스터 공격 텍스트 패널 출력
                    for (int i = 0; i < counterMessages[0].Count; i++)
                    {
                        var panel = new Panel(counterMessages[1][i]);
                        panel.Border = BoxBorder.Rounded;
                        // 패널 상단 중앙에 제목 적기(왼쪽, 오른쪽으로도 변경 가능)
                        panel.Header = new PanelHeader(counterMessages[0][i], Justify.Center);
                        // 패널 출력
                        AnsiConsole.Write(panel);
                    }

                    // 플레이어가 사망하면 계산 종료하고 던전 빠져나가게끔
                    if (Program.player.hp <= 0)
                        return;

                    // 다음 몬스터 공격까지 딜레이
                    Thread.Sleep(500);
                }
            }
            AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("다음\n"));
        }
    }
}
