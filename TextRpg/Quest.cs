using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

    // 1) 몬스터 총합 n마리 처치
    // 2) 특정 몬스터 n마리 처치
    // 3) 특정 던전 n회 달성
    // 4) 레벨 n 달성
    // 5) 골드 n 획득
    // 6) 튜토리얼: 장비 장착해보기
    // 7) 튜토리얼: 아이템 사용해보기
    // 더 있는지 생각해보기


    // 3. 부터는 내일 코드 마저 작성

    // 3. 어디서 달성 여부를 체크하는지?
    // 보통은 퀘스트를 주는 NPC에게 가거나,
    // 업적 창에서 달성 버튼을 누르거나(화면 전환 때 달성한 업적 버튼 달성 활성화)
    // 즉, 특정 트리거가 있으면 달성 기준을 체크하게 하는 듯함
    // 그러면, 퀘스트 창(하나라도 달성이 되었다면 달성 표시 뜨게끔 표시. 해당 구문 만들고 다른 씬 작업자님들과 합의)에 진입 >> 달성 퀘스트(표시 필요) 세부 확인 >> 보상

    // 4. 데이터 저장(나중에 만들 예정)
    // 각 퀘스트에 대해 딕셔너리 하나로 처리하는 방법을 검토
    // Dictionary<int, int>
    // >> 퀘스트 인덱스(key), 퀘스트 count 값(value)
    // >> 완료한 퀘스트는 key를 제외 >> 없는 key는 이미 완료한 퀘스트!
    // >> 퀘스트를 완료할수록 체크할 양과 저장 데이터가 줄어들어서 점점 좋아지는 구조

    // 다만, 라이브 서비스의 경우 추가 업적을 만들 때 유저 모든 데이터에 추가 퀘스트들에 대해 인덱스, count값 페어를 넣어줘야 함
    // 싱글 게임 dlc의 경우는 그냥 실행하는 각 플레이어 데이터에 대해 처리해주면 되기에 큰 문제가 없음..
    // 어??? 생각해보니 라이브 서비스 게임도 이러면 되긴 하겠네
    // 대신 퀘스트 클리어 여부를 키 보유 여부로만 따지면 안되고
    // 적어도 각 퀘스트 완료 여부를 나타내는 비트들의 모음 정도는 있어야 할 듯 >> 정보 처리 속도도 빠르고 확실한 지표가 될 수 있음

    internal class Quest
    {
        internal static QuestForm[] quests = new QuestForm[] { }; // 빈 배열로 초기화. 배열 크기를 지정하지 않으면 동적 배열

        public Quest() // 퀘스트 초기화
        {
            QuestForm questTmp = new QuestForm(); // 임시 퀘스트폼

            // 생성 초기에 공통적으로 달성하지 않은 상태
            questTmp.isAccomplish = false; 
            questTmp.count = 0;

            questTmp.title = "마을을 위협하는 미니언 처치\n";
            questTmp.info = "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\n" +
                            "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n" +
                            "모험가인 자네가 좀 처치해주게!\n";
            questTmp.goalInfo = " - 미니언 5마리 처치 ({0}/{1})\n";
            questTmp.goal = 5;
            questTmp.rewards.exp = 0;
            questTmp.rewards.gold = 5;
            // 퀘스트 클리어 후 아이템 지급 테스트 용도로 임의로 상점에 팔지 않는 아이템 하나 생성
            questTmp.rewards.items = new RewardItem[] { new RewardItem(new Armor("초심자 갑옷", 1, "최소한의 보호 장비", 50, false), 1) };
            quests[0] = questTmp; // 구조체는 값을 참조하기에 만들어진 QuestForm으로부터 배열 원소에 값을 집어넣게끔

            // 이후에 questTmp가 변하여도 값을 집어넣은 quests[0]는 바뀌지 않음. 이하 동일
            questTmp.title = "장비를 장착해보자";
            questTmp.info = "훌륭한 모험가는 좋은 장비를 착용하는 법이지\n" +
                            "맨손으로 몬스터에게 맞선다고? 자네 제정신인가?\n" +
                            "집에 무기라도 쓸 만한 게... 어디 이거라도 좀 들어보게나\n";
            questTmp.goalInfo = " - 장비 착용해보기 ({0}/{1})\n";
            questTmp.goal = 1;
            questTmp.rewards.exp = 10;
            questTmp.rewards.gold = 0;
            questTmp.rewards.items = null;
            quests[1] = questTmp;

            questTmp.title = "더욱 더 강해지기";
            questTmp.info = "모험가 협회에서는 모험가들에게 지원을 하고 있다네\n" +
                            "일정 수준 이상의 모험가들은 협회에도 귀한 자원이니 말일세\n" +
                            "레벨 5를 달성한다면 다시 오게나\n";
            questTmp.goalInfo = " - 레벨 달성 ({0}/{1})\n";
            questTmp.goal = 5;
            questTmp.rewards.exp = 0;
            questTmp.rewards.gold = 1000;
            questTmp.rewards.items = null;
            quests[2] = questTmp;
        }


        //int quest_max = 3; // 한번에 표시할 수 있는 퀘스트 최대 갯수

        // 퀘스트 씬0 : 퀘스트 목록들 출력
        internal void ShowQuestList()
        {
            WriteLine_Color("Quest!!\n", ConsoleColor.Yellow);
            for (int i = 0; i < quests.Length; i++)
            {
                Write_Color($"{i + 1}. ", ConsoleColor.Red); // 퀘스트 순번(+해당 번호 입력 시 내용 확인 가능하다고 알림)
                Console.WriteLine(quests[i].title); // 퀘스트 요약 타이틀 표시
            }
        }

        // 퀘스트 씬1 : 선택한 퀘스트의 상세 정보 출력
        internal void ShowQuestDetail(int index)
        {
            WriteLine_Color("Quest!!\n", ConsoleColor.Yellow);
            Console.WriteLine(quests[index].title); // 퀘스트 요약 타이틀
            Console.WriteLine(quests[index].info); // 퀘스트 설명
            Console.WriteLine(quests[index].goalInfo, quests[index].count, quests[index].goal); // 목표 설명                                                                
            PrintQuestRewards(index);// 보상 설명
        }

        // 임시 퀘스트 씬 : 퀘스트 완료 창을 보여주고 아무 키 입력으로 원래대로 돌아감
        // 퀘스트가 완료되면 즉시 나왔다가 키 입력으로 사라짐
        internal void AlarmAccomplish(int questIndex)
        {
            // 새로운 콘솔 출력창(새 창을 띄우진 않으나 새 문서 같은 느낌으로 덮어써서 보이게끔)
            StringWriter stringWriter = new StringWriter();

            // 기존 콘솔 출력창
            TextWriter originalConsoleOut = Console.Out;

            // 새로운 콘솔 출력창 출현(비어 있음)
            Console.SetOut(stringWriter);

            // 퀘스트 달성 및 보상 알림
            Console.WriteLine("퀘스트 달성\n\n{0}\n\n", quests[questIndex].title);
            PrintQuestRewards(questIndex);

            Console.WriteLine("뒤로 가려면 아무 키나 눌러주세요.");
            Console.ReadKey(); // 아무 키나 입력할 때까지 대기

            // 입력으로 다른 화면 코드와 충돌할지 모르니 시간 대기도 옵션으로 만들어 두었습니다.
            //Thread.Sleep(1000); // 1초간 대기 (대기 도중에 입력을 받아도 버퍼에만 쌓일 뿐 그 동안 다른 스크립트에서 작동하지 않습니다.)
            // 입력 버퍼 비워주기 (대기 도중에 입력한 키 무효)
            //while (Console.KeyAvailable)
            //{
            //    Console.ReadKey(true); // 매개변수를 true로 설정하면 키를 하나씩 읽으면서 콘솔에는 표시하지 않음
            //}

            Console.SetOut(originalConsoleOut); // 다시 원래 콘솔 출력창으로
        }

        void PrintQuestRewards(int questIndex)
        {
            // 퀘스트 보상 골드, 경험치 출력
            Console.WriteLine("GOLD : {0} G\nEXP : {1} Exp\n\n", quests[questIndex].rewards.gold, quests[questIndex].rewards.exp);
            if (quests[questIndex].rewards.items != null)
            {
                // 퀘스트 보상 아이템들 목록 전체에 대해
                for (int i = 0; i < quests[questIndex].rewards.items.Length; i++)
                {
                    // 아이템 이름 x 갯수 출력
                    Console.WriteLine($"{quests[questIndex].rewards.items[i].item.itemName} x {quests[questIndex].rewards.items[i].amount}"); 
                }
                Console.WriteLine(); // 한줄 더 띄워서 아래와 붙지 않도록
            }
        }

        // 다른 글자색을 표현하기 위한 메서드들
        internal void WriteLine_Color(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color; // 앞으로 사용할 글자 색상 교체
            Console.WriteLine(text); // 바꾼 글자 색으로 출력하고 한줄 띄우기
            Console.ResetColor(); // 색상 원복
        }
        internal void Write_Color(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color; // 앞으로 사용할 글자 색상 교체
            Console.Write(text); // 바꾼 글자 색으로 출력하고
            Console.ResetColor(); // 색상 원복
        }
    }

    struct QuestForm
    {
        internal string
            title, // 퀘스트 명칭
            info,  // 퀘스트에 대한 상세한 설명
            goalInfo; // 퀘스트 목표

        internal int 
            count, // 달성도
            goal; // 목표치

        internal bool isAccomplish; // 퀘스트 달성 여부

        internal Rewards rewards; // 보상

        // 달성량 추가 후 퀘스트 달성 여부 체크 >> 각 필요한 화면에서 불러오기
        internal void CheckAccomlish(int addCount)
        { 
            if (isAccomplish) // 이미 달성한 퀘스트라면 메서드를 종료
                return;

            count += addCount; // 카운트 증가
            if(count >= goal) // 달성하지 않은 퀘스트의 목표를 달성했을 때 
            {  
                isAccomplish = true; // 달성 상태 기록
                Reward(); // 보상 지급
            }
        }

        // 퀘스트 보상 받기
        internal void Reward()
        {
            Program.player.gold += rewards.gold;    // 골드
            Program.player.exp += rewards.exp;      // 경험치
            // 경험치 올라갔을 때 레벨업이 되는지 체크 >> 만들어진 로직에 잇기

            // 아이템
            if (rewards.items != null)  // 보상 아이템이 있다면
            {
                // 보상 아이템들을 인벤토리에 추가
                for(int i = 0;i < rewards.items.Length;i++)
                {
                    for (int j = 0; j < rewards.items[i].amount; j++)
                    {
                        Program.inventory.inventory.Add(rewards.items[i].item); // 갯수에 맞게 각 아이템 획득
                    }
                }
            }
        }

        // 현재 보상은 크게 골드, 경험치, 아이템 셋으로 나뉩니다.
        // 아이템은 동시에 여러 종류를 줄 수 있게 이와 같이 만들었습니다.
        internal struct Rewards
        {
            internal int gold;
            internal int exp;
            internal RewardItem[]? items;
        }

        // 보상 아이템 정보
        // 아이템, 갯수
        internal struct RewardItem
        {
            internal Item item;
            internal int amount;

            internal RewardItem(Item _item, int _amount)
            {
                item = _item;
                amount = _amount;
            }
        }
    }
}
