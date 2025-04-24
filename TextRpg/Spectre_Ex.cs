using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace TextRpg
{
    internal class Spectre_Ex
    {
        static void Print_Spectre_Examples()
        {
            // 1. Markup 텍스트
            // 적용할 문자열 앞에 대괄호[], 이 안에 꾸며줄 방법을 쓰면 됩니다.
            // (꾸며주는 내용은 여럿 함께 사용 가능. 단, 색상은 중복 사용 불가 >> 에러 뜸..)
            // >> 적용할 문자열이 끝나면 [/]로 적용 범위의 끝을 적어주면 완료!

            // 1) 색상 : red, green, blue, yellow, cyan, magenta, white, gray, black, #ffffff(Hex 색상 코드로도 지정 가능)
            // 2) 스타일 : bold, dim, italic, underline, strikethrough, reverse

            AnsiConsole.WriteLine("1. 글자색 다른 문자열 출력");
            AnsiConsole.WriteLine("1) 색상");
            AnsiConsole.Markup("[red]빨강[/], [green]초록[/], [blue]파랑[/],\n" +
                               "[yellow]노랑[/], [cyan]청록[/], [magenta]핑크[/],\n" +
                               "[white]하양[/], [gray]회색[/], [black]검정[/]," +
                               "\n[#ca64f4]보라색 #ca64f4[/]\n\n");

            AnsiConsole.MarkupLine("2) 스타일");
            AnsiConsole.MarkupLine("보통, [bold]굵음[/], [dim]흐림[/],\n" +
                   "[italic]기울임[/], [underline]밑줄[/], [strikethrough]취소선[/],\n" +
                   "[reverse]배경과 텍스트 색 반전[/]\n");

            AnsiConsole.MarkupLine("3) 여러 스타일 조합도 가능!");
            AnsiConsole.MarkupLine("[bold underline red]굵은 빨간 밑줄 텍스트[/]\n\n");

            // 2. 표
            AnsiConsole.MarkupLine("2. 표");
            var table = new Table(); // 테이블 객체 생성
            // 항목
            table.AddColumn("종류");
            // table.AddColumn("값");
            table.AddColumn(new TableColumn("값").Centered()); // 해당 칸에 중앙정렬할 때는 이와 같이 사용
            // 항목에 맞는 표 내용
            table.AddRow("[green]체력[/]", "10 / 10"); // Markup 텍스트 사용 가능!
            table.AddRow("공격력", "10");
            table.AddRow("방어력", "5");
            // 아래 구문을 풀면 표도 창 중간에 정렬 가능
            //table.Centered();
            // 표 출력
            AnsiConsole.Write(table);


            // 3. 박스 스타일 패널
            AnsiConsole.MarkupLine("\n\n3. 패널");
            var panel = new Panel("[yellow]던전 뿌셔[/]\n\n<< 보상 >>\n경험치: +100 Exp\n골드: +1000G\n\n아이템\n스파르타 무언가"); // 패널 생성 및 안에 들어갈 내용
            // 테두리 스타일(Rounded, Square, Ascii, None 등) Double, Heavy는 제대로 출력이 안되는 듯함
            panel.Border = BoxBorder.Rounded;
            // 패널 상단 중앙에 제목 적기(왼쪽, 오른쪽으로도 변경 가능)
            panel.Header = new PanelHeader("[yellow bold]퀘스트 완료[/]", Justify.Center);
            // 패널 출력
            AnsiConsole.Write(panel);



            // 4. 진행 바
            AnsiConsole.MarkupLine("\n\n4. 진행바");
            AnsiConsole.Progress().Start(x =>
            {
                var task = x.AddTask("[red]던전 진행 중...[/]");
                while (!task.IsFinished) // task가 끝나지 않았을 때
                {
                    task.Increment(5); // 5%씩 증가, 100%가 최대
                    Thread.Sleep(100); // 딜레이
                }
            }
            );
            AnsiConsole.MarkupLine("\n위는 진행도가 어떻게 차는지 자동으로 보여주기 위해 while문을 사용했으나\n 그냥 특정 진행도를 표시해도 무방");
            AnsiConsole.MarkupLine("\n[bold yellow]퀘스트 달성도[/]");
            AnsiConsole.Progress().Start(x =>
            {
                var task = x.AddTask("[italic yellow]몬스터 처치 :[/]");
                int goal = 5;
                int count = 1;
                float percentage = (float)count * 100 / goal; // 진행도를 백분률로
                task.Increment(percentage); // 진행도 만큼 증가
            }
);

            // 5. 사용자 입력 프롬프트
            // 선택지 혹은 답변 입력

            // 1) TextPrompt<T>() : 사용자 입력
            AnsiConsole.MarkupLine("\n\n5. 입력 프롬프트\n1) 입력");
            var name = AnsiConsole.Prompt(
                       new TextPrompt<string>("[bold]이름을 입력하세요:[/]")
                       .PromptStyle("white")
                       .DefaultValue("[dim gray]예) 전붕이[/]")
                       .AllowEmpty());
            Console.WriteLine($"입력한 이름: {name}");

            // 2) SelectionPrompt<T>() : 여러 선택지 중 하나를 선택하게 함
            AnsiConsole.MarkupLine("\n\n2) 선택지");
            var menu = AnsiConsole.Prompt(
                       new SelectionPrompt<string>()
                       .Title("무엇을 하시겠습니까?")
                       .PageSize(5)
                       .AddChoices(new[] { "이어하기", "새로하기", "설정", "종료" }));
            Console.WriteLine($"선택한 메뉴: {menu}");

            // 3) MultiSelectionPrompt<T>() : 선택지 다중 선택 가능
            AnsiConsole.MarkupLine("\n\n3) 다중 선택지");
            var items = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                        .Title("가방에 넣을 아이템을 선택해주세요.")
                        .NotRequired()
                        .PageSize(4)
                        .InstructionsText("[grey](스페이스바로 선택, 엔터로 완료)[/]")
                        .AddChoices("HP포션", "MP포션", "돌", "잡초"));

            Console.WriteLine("선택한 아이템:");
            foreach (var item in items)
                Console.WriteLine($"- {item}");

            // 6. 트리
            AnsiConsole.MarkupLine("\n\n6. 트리");
            var tree = new Tree("[green]던전 몬스터[/]");

            var branch1 = tree.AddNode("[yellow]1층 몬스터[/]");
            branch1.AddNode("[red]1) 박쥐[/]");
            branch1.AddNode("[red]2) 슬라임[/]");

            var branch2 = tree.AddNode("[yellow]2층 몬스터[/]");
            branch2.AddNode("[red]1) 스켈레톤[/]");
            branch2.AddNode("[red]2) 지나가던 촌장[/]");

            AnsiConsole.Write(tree);
        }
    }
}
