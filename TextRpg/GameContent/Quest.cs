using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using static TextRpg.QuestForm;

namespace TextRpg
{
    //1. 퀘스트 구조 분석
    //1) 퀘스트 타이틀(string)
    //2) 퀘스트 설명(string)
    //3) 퀘스트 목표 설명(string)
    //4) 퀘스트 목표(int)
    //5) 퀘스트 달성치(int)
    //6) 퀘스트 보상(gold, exp, Item[])
    //7) 퀘스트 달성 여부(bool)


    // 2. 어떤 퀘스트를 만들까?
    // 막연하니 기존의 사례들로부터 참고해보자

    // 1) 몬스터 총합 n마리 처치 >> 전투 결과창에 합산
    // 2) 특정 몬스터 n마리 처치 >> 전투 결과창에 합산(몬스터 별 드랍이 있기에 어떤 몬스터인지 기억해둘 듯)
    // 3) 특정 던전 n회 달성 >> 전투 결과창
    // 4) 레벨 n 달성 >> 전투 결과창 + 퀘스트 보상을 받을 때(경험치가 보상으로 들어가는 퀘스트가 있음) >> 플레이어에 GetExp()로 레벨업까지 한번에 처리하게 하면 거기서 체크하면 해결됨
    // 5) 골드 n 획득 >> 전투 결과창 + 퀘스트 보상을 받을 때(골드가 보상으로 들어가는 퀘스트가 있음) >> 플레이어에 GetGold()로 일괄 처리 + 
    // 6) 튜토리얼: 장비 장착해보기
    // 7) 튜토리얼: 아이템 사용해보기
    // 8) 튜토리얼: 스킬 사용해보기

    // 생각은 했으나 지금은 컨텐츠를 만들기보다
    // 이후 얼마든지 쉽게 추가(유지보수) 가능한 구조를 만드는 걸 해보는 것이 중요하다 판단
    // 발제 자료에 있는 것으로만 제작


    // 3. 어디서 달성 보상을 받는지?
    // 1) 퀘스트를 주는 NPC에게 가거나,
    // 2) 업적 창에서 달성 버튼을 누르거나(화면 전환 때 달성한 업적 버튼 달성 활성화)
    // 3) 달성 즉시
    // 크게 2가지 종류
    // (1) 특정 트리거를 통해 달성 보상을 받게끔
    // (2) 완료 즉시 달성 보상이 뜨게끔 >> 이걸로 하겠습니다.
    // 데이터는 어디서 쌓을까? >> 각 데이터가 변하는 위치에서 메서드 하나로 처리

    // 4. 데이터 저장(나중에 만들 예정)
    // 각 퀘스트에 대해 딕셔너리 하나로 처리하는 방법을 검토
    // Dictionary<int, int>
    // >> 퀘스트 인덱스(key), 퀘스트 count 값(value)
    // >> 완료한 퀘스트는 key를 제외 >> 없는 key는 이미 완료한 퀘스트!
    // >> 퀘스트를 완료할수록 체크할 양과 저장 데이터가 줄어들어서 점점 좋아지는 구조

    public class Quest
    {
        List<QuestForm> quests = new List<QuestForm>() { };

        // 저장/불러오기용 퀘스트 데이터(json에 필요하면 public으로 변경해도 괜찮습니다)
        Dictionary<int, int> questData = new Dictionary<int, int>();

        public Dictionary<int, int> Get_questData() { return questData; }
        internal void Set_questData(int questNum, int count)
        {
            if(questData.ContainsKey(questNum))
                questData[questNum] = count;
        }

        public Quest() // 퀘스트 초기화
        {
            QuestForm questTmp = new QuestForm();// 임시 퀘스트폼
            int index = 0;

            #region 퀘스트 데이터
            ////////////////////////////////////////////////////////////////// 한 블럭 복사하여 새 퀘스트 추가 가능
            questTmp = new QuestForm();// 임시 퀘스트폼
            // 안내문
            questTmp.title = "마을을 위협하는 몬스터 처치";
            questTmp.info = "이봐! 마을 근처에 몬스터들이 너무 많아졌다고 생각하지 않나?\n" +
                            "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n" +
                            "모험가인 자네가 좀 처치해주게!\n";
            questTmp.goalInfo = " - 몬스터 5마리 처치";
            // 목표
            questTmp.goal = 5;
            // 카운트 방식 (true: 받은 값 써주기, false: 받은 값 더하기)
            questTmp.isOverrideCount = false;
            // 보상
            questTmp.rewards.exp = 0;
            questTmp.rewards.gold = 5;
            // 퀘스트 클리어 후 아이템 지급 테스트 용도로 임의로 상점에 팔지 않는 아이템 하나 생성
            questTmp.rewards.items = new RewardItem[] { new RewardItem(new Armor("초심자 갑옷", 1, "최소한의 보호 장비", 50, false), 1) };
            // 퀘스트 리스트에 추가
            quests.Add(questTmp);


            //////////////////////////////////////////////////////////////////////// 한 블럭 복사하여 새 퀘스트 추가 가능
            questTmp = new QuestForm();// 임시 퀘스트폼
            // 안내문
            questTmp.title = "장비를 장착해보자";
            questTmp.info = "훌륭한 모험가는 좋은 장비를 착용하는 법이지\n" +
                            "맨손으로 몬스터에게 맞선다고? 자네 제정신인가?\n" +
                            "아무 장비라도 하나 걸쳐보게\n";
            questTmp.goalInfo = " - 장비 착용해보기";
            // 목표
            questTmp.goal = 1;
            // 카운트 방식 (true: 받은 값 써주기, false: 받은 값 더하기)
            questTmp.isOverrideCount = true;
            // 보상
            questTmp.rewards.exp = 10;
            questTmp.rewards.gold = 0;
            questTmp.rewards.items = null;
            // 퀘스트 리스트에 추가
            quests.Add(questTmp);

            /////////////////////////////////////////////////////////////////////////// 한 블럭 복사하여 새 퀘스트 추가 가능
            questTmp = new QuestForm();// 임시 퀘스트폼
            // 안내문
            questTmp.title = "더욱 더 강해지기";
            questTmp.info = "모험가 협회에서는 모험가들에게 지원을 하고 있다네\n" +
                            "일정 수준 이상의 모험가들은 협회에도 귀한 자원이니 말일세\n" +
                            "레벨 5를 달성한다면 다시 오게나\n";
            questTmp.goalInfo = " - 레벨 달성";
            // 목표
            questTmp.goal = 5;
            // 카운트 방식 (true: 받은 값 써주기, false: 받은 값 더하기)
            questTmp.isOverrideCount = true;
            // 보상
            questTmp.rewards.exp = 0;
            questTmp.rewards.gold = 1000;
            questTmp.rewards.items = null;
            // 퀘스트 리스트에 추가
            quests.Add(questTmp);

            #endregion
        }

        public void InitQuestData()
        {
            // 모든 퀘스트에 대해
            for (int i = 0; i < quests.Count; i++)
            {
                // 생성 초기에 공통적으로 달성하지 않은 상태
                quests[i].isAccomplish = false;
                quests[i].count = 0;

                // 새 저장 데이터 생성
                questData.Add(i, 0);
            }
        }

        public void LoadQuestData()
        {
            // 모든 퀘스트에 대해
            for (int i = 0; i < quests.Count; i++)
            {
                // 해당 퀘스트 번호를 키로 가지고 있다면
                if (questData.ContainsKey(i))
                {
                    // 해당 퀘스트 미완
                    quests[i].isAccomplish = false;
                    // 퀘스트 달성도 값 불러오기
                    questData.TryGetValue(i, out quests[i].count);
                }
                // 해당 퀘스트 번호가 없다면
                else
                {
                    // 해당 퀘스트 완료
                    quests[i].isAccomplish = true;
                    // 목표값으로 덮어쓰기
                    quests[i].count = quests[i].goal;
                }
            }
        }

        // 퀘스트 씬0 : 퀘스트 목록들 출력 >> 마을에서 볼 수 있게끔 하기
        public void PrintQuestList()
        {
            Console.Clear(); // 다른 화면에서 남은 부분이 있을지 모르니 Clear()
            AnsiConsole.Write(new Rule("[green]퀘스트를 성공해보세요[/]"));

            // 선택지 리스트
            List<string> questMenu = new List<string>();
            for (int i = 0; i < quests.Count; i++)
            {
                string thisMenu = $"[red]{ i + 1}.[/] {quests[i].title}";
                if (quests[i].isAccomplish) // 퀘스트 달성 여부 표시
                    thisMenu += "[red] (달성)[/]";
                if (i == quests.Count - 1) // 마지막 메뉴 아래에 한줄 띄워주기
                    thisMenu += "\n";
                questMenu.Add(thisMenu);
            }
            questMenu.Add("나가기"); // 나가기 메뉴도 추가

            // 선택지 프롬프트(여기서 입력 대기)
            var menu = AnsiConsole.Prompt(
           new SelectionPrompt<string>()
           .PageSize(quests.Count+1) // 퀘스트 수 + 나가기 메뉴
           .AddChoices(questMenu)
           .WrapAround());

            
            // 선택한 메뉴가 몇번째인가?
            int index = 0;
            foreach (var menus in questMenu)
            {
                if (menus == menu)
                    break;
                else
                    index++;
            }
            if(index < quests.Count)
                PrintQuestDetail(index);
            // 나가기를 선택했다면 자동으로 마을로
        }

        // 퀘스트 씬1 : 선택한 퀘스트의 상세 정보 출력
        public void PrintQuestDetail(int index)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[yellow]Quest!![/]\n");
            Console.Write(quests[index].title); // 퀘스트 요약 타이틀
            if(quests[index].isAccomplish)
                AnsiConsole.Markup("[red] (달성)[/]");
            Console.WriteLine("\n");
            Console.WriteLine(quests[index].info); // 퀘스트 설명
            AnsiConsole.MarkupLine(quests[index].goalInfo + $" [red]({quests[index].count}/{quests[index].goal})[/]"); // 목표 설명                                                                
            // 퀘스트 보상 골드, 경험치 출력
            AnsiConsole.Markup("GOLD : [yellow]{0}[/] G\nEXP : [yellow]{1}[/] Exp\n\n", quests[index].rewards.gold, (int)quests[index].rewards.exp);
            if (quests[index].rewards.items != null)
            {
                // 퀘스트 보상 아이템들 목록 전체에 대해
                for (int i = 0; i < quests[index].rewards.items.Length; i++)
                {
                    // 아이템 이름 x 갯수 출력
                    AnsiConsole.MarkupLine($"[yellow]{quests[index].rewards.items[i].item.ItemName} x {quests[index].rewards.items[i].amount}[/]");
                }
                Console.WriteLine(); // 한줄 더 띄워서 아래와 붙지 않도록
            }
            Console.WriteLine();
            // 선택지 프롬프트(여기서 입력 대기)
            string menu = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("나가기\n")); // 줄바꿈이 없으면 나가기가 2줄 출력..???
            // 입력을 받으면 퀘스트 목록으로 나가기
            PrintQuestList();
        }

        // 퀘스트 달성도 경신 후 달성 여부 체크
        // 0: 전투 결과 때 몬스터 출현 수를 더하기 >> 던전 마스터님의 작업이 끝나면 결과창에 추가하기!!!!!
        // 1: 장비 장착할 때 1
        // 2: 플레이어 레벨업 메서들 호출 때마다
        public void QuestRenewal(int questIndex, int countChanged)
        {
            quests[questIndex].CheckAccomlish(questIndex, countChanged);
        }
    }

    class QuestForm
    {
        public string?
            title, // 퀘스트 명칭
            info,  // 퀘스트에 대한 상세한 설명
            goalInfo; // 퀘스트 목표

        public int 
            count, // 달성도
            goal; // 목표치

        public bool
            isAccomplish, // 퀘스트 달성 여부
            isOverrideCount; // 카운트 방식(true: 값 덮어쓰기, false: 값 축적) // 어떤 구조일지 모르는 부분들이 있어 변수 하나로 대처하기 위함

        public Rewards rewards; // 보상

        // 달성량 추가 후 퀘스트 달성 여부 체크 >> 각 필요한 화면에서 불러오기
        public void CheckAccomlish(int questIndex, int addCount)
        { 
            if (isAccomplish) // 이미 달성한 퀘스트라면 메서드를 종료
                return;

            // 카운트 방식에 따른 경신
            if (isOverrideCount)
                count = addCount; 
            else
                count += addCount;
            // 달성도 저장
            Program.quest.Set_questData(questIndex, count);
            // 달성하지 않은 퀘스트의 목표를 달성했을 때 
            if (count >= goal) 
            {
                isAccomplish = true; // 달성 상태 기록
                Program.quest.Get_questData().Remove(questIndex); // 달성한 퀘스트는 데이터 딕셔너리에서 제거
                Reward(); // 보상 지급
                AlarmAccomplish(); // 보상 지급 메시지 출력
            }
        }

        // 퀘스트 보상 받기
        public void Reward()
        {
            Program.player.gold += rewards.gold;    // 골드
            Program.player.AddExp(rewards.exp);      // 경험치

            // 아이템
            if (rewards.items != null)  // 보상 아이템이 있다면
            {
                // 보상 아이템들을 인벤토리에 추가
                for(int i = 0;i < rewards.items.Length;i++)
                {
                    // 각 아이템 갯수에 맞게 획득
                    for (int j = 0; j < rewards.items[i].amount; j++)
                    {
                        Program.inventory.getInventory().Add(rewards.items[i].item); 
                    }
                }
            }
        }


        // 임시 퀘스트 씬을 만들려 했으나,
        // 콘솔 화면을 임시 저장하는 기능은 없는 것으로 판명
        // 따라서 기존 화면을 다시 불러올 수 있어야 하는데. 그럴려면 씬 번호라던지 식별할 수 있는 부분이 있어야 함
        // 그냥 추가 메세지로 완료했다고 띄워주는 것으로 변경하기 >> 꾸밀 때 패널로 구분해줘도 좋다고 생각 
        public void AlarmAccomplish()
        {
            string questResult = $"[yellow]{title}[/]\n\n<< 보상 >>\n골드: +{rewards.gold} G\n경험치: +{(int)rewards.exp} Exp\n";
            if (rewards.items != null)
            {
                questResult += "\n아이템\n";
                // 퀘스트 보상 아이템들 목록 전체에 대해
                for (int i = 0; i < rewards.items.Length; i++)
                {
                    // 아이템 이름 x 갯수 출력
                    questResult += $"{rewards.items[i].item.ItemName} x {rewards.items[i].amount}\n";
                }
            }
            var panel = new Panel(questResult); // 패널 생성 및 안에 들어갈 내용
            panel.Border = BoxBorder.Rounded;
            panel.Header = new PanelHeader("[yellow bold]퀘스트 완료[/]", Justify.Center);
            Console.WriteLine();
            AnsiConsole.Write(panel);
            Console.WriteLine();
        }

        // 현재 보상은 크게 골드, 경험치, 아이템 셋으로 나뉩니다.
        // 아이템은 동시에 여러 종류를 줄 수 있게 이와 같이 만들었습니다.
        public struct Rewards
        {
            public int gold;
            public double exp;
            public RewardItem[]? items;
        }

        // 보상 아이템 정보
        // 아이템, 갯수
        public struct RewardItem
        {
            public Item item;
            public int amount;

            public RewardItem(Item _item, int _amount)
            {
                item = _item;
                amount = _amount;
            }
        }
    }
}
