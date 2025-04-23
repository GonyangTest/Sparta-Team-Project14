using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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


    // 3. 부터는 내일 코드 마저 작성

    // 3. 어디서 달성 보상을 받는지?
    // 1) 퀘스트를 주는 NPC에게 가거나,
    // 2) 업적 창에서 달성 버튼을 누르거나(화면 전환 때 달성한 업적 버튼 달성 활성화)
    // 3) 달성 즉시
    // 크게 2가지 종류
    // (1) 특정 트리거를 통해 달성 보상을 받게끔
    // (2) 완료 즉시 달성 보상이 뜨게끔
    // 흚... 데이터를 쌓다가 달성하면 즉시 뜨게끔 하는 게 좋지 않을까?
    // 데이터는 어디서 쌓을까?

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

    public class Quest
    {
        List<QuestForm> quests = new List<QuestForm>() { };

        // 저장/불러오기용 퀘스트 데이터(json에 필요하면 public으로 변경해도 괜찮습니다)
        Dictionary<int, int> questData = new Dictionary<int, int>();

        public Quest() // 퀘스트 초기화
        {
            QuestForm questTmp = new QuestForm();// 임시 퀘스트폼
            int index = 0;

            ReSetForm(ref questTmp, ref index); // 임시 퀘스트폼 공통 부분 초기화
            questTmp.title = "마을을 위협하는 몬스터 처치";
            questTmp.info = "이봐! 마을 근처에 몬스터들이 너무 많아졌다고 생각하지 않나?\n" +
                            "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n" +
                            "모험가인 자네가 좀 처치해주게!\n";
            questTmp.goalInfo = " - 몬스터 5마리 처치 ({0}/{1})\n";
            questTmp.goal = 5;
            questTmp.rewards.exp = 0;
            questTmp.rewards.gold = 5;
            // 퀘스트 클리어 후 아이템 지급 테스트 용도로 임의로 상점에 팔지 않는 아이템 하나 생성
            questTmp.rewards.items = new RewardItem[] { new RewardItem(new Armor("초심자 갑옷", 1, "최소한의 보호 장비", 50, false), 1) };
            quests.Add(questTmp); // 리스트는 값을 참조 >> 추가할 때 questTmp의 값을 복사하여 새로운 원소로 만듦

            ReSetForm(ref questTmp, ref index);
            questTmp.title = "장비를 장착해보자";
            questTmp.info = "훌륭한 모험가는 좋은 장비를 착용하는 법이지\n" +
                            "맨손으로 몬스터에게 맞선다고? 자네 제정신인가?\n" +
                            "아무 장비라도 하나 걸쳐보게\n";
            questTmp.goalInfo = " - 장비 착용해보기 ({0}/{1})\n";
            questTmp.goal = 1;
            questTmp.rewards.exp = 10;
            questTmp.rewards.gold = 0;
            questTmp.rewards.items = null;
            quests.Add(questTmp);

            ReSetForm(ref questTmp, ref index);
            questTmp.title = "더욱 더 강해지기";
            questTmp.info = "모험가 협회에서는 모험가들에게 지원을 하고 있다네\n" +
                            "일정 수준 이상의 모험가들은 협회에도 귀한 자원이니 말일세\n" +
                            "레벨 5를 달성한다면 다시 오게나\n";
            questTmp.goalInfo = " - 레벨 달성 ({0}/{1})\n";
            questTmp.goal = 5;
            questTmp.rewards.exp = 0;
            questTmp.rewards.gold = 1000;
            questTmp.rewards.items = null;
            quests.Add(questTmp);
        }

        void ReSetForm(ref QuestForm tmpForm, ref int index)
        {
            // !!!!! 저장 데이터가 있다면
            // index 번째의 퀘스트가 달성 >> tmpForm.isAccomplish = true; tmpForm.count = (데이터 값); 을 받기(당장 쓰지 않더라도 나중에 확장하면 쌓인 데이터로 바로 퀘스트 클리어 가능하게끔 할 수 있음. 계속 플레이한 유저들에게 효용감을 줄 수 있음)
            // 달성하지 못했다면 tmpForm.isAccomplish = false; tmpForm.count = (데이터 값); 을 받기

            // 저장 데이터가 없다면
            // 생성 초기에 공통적으로 달성하지 않은 상태
            tmpForm.isAccomplish = false;
            tmpForm.count = 0;

            // 다음 저장 데이터로
            index += 1;
        }

        // 퀘스트 씬0 : 퀘스트 목록들 출력 >> 마을에서 볼 수 있게끔 하기
        public void PrintQuestList()
        {
            Console.Clear(); // 다른 화면에서 남은 부분이 있을지 모르니 Clear()
            WriteLine_Color("Quest!!\n", ConsoleColor.Yellow);
            for (int i = 0; i < quests.Count; i++)
            {
                Write_Color($"{i + 1}. ", ConsoleColor.Red); // 퀘스트 순번(+해당 번호 입력 시 내용 확인 가능하다고 알림)
                Console.WriteLine(quests[i].title); // 퀘스트 요약 타이틀 표시
            }
            Console.WriteLine();
            Console.Write("0. 나가기\n\n원하시는 퀘스트를 선택해주세요 >> ");

            while (true)
            {
                string next = Console.ReadLine();
                int next_int;
                if (!int.TryParse(next, out next_int))
                {
                    Console.Clear();
                    Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                    Console.ReadKey();
                }
                else
                {
                    // 0번: 뒤로가기(타운 씬으로 이동)
                    if (next_int == 0)
                    {
                        Console.Clear();
                        // !!!!! 타운 씬으로 이동하는 구문 넣기
                        break;
                    }

                    // 1~퀘스트 갯수까지 >> 해당 퀘스트 상세 확인으로 이동
                    else if(next_int <= quests.Count && next_int > 0)
                    {
                        PrintQuestDetail(next_int);
                        break;
                    }

                    // 유효하지 않은 입력들에 대해
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                        Console.ReadKey();
                    }
                }
            }
        }

        // 퀘스트 씬1 : 선택한 퀘스트의 상세 정보 출력
        public void PrintQuestDetail(int index)
        {
            index -= 1; // 퀘스트 항목 선택은 1부터 시작. 그러나 퀘스트 데이터는 0부터 시작하기에 차이 1만큼 빼주기
            Console.Clear();
            WriteLine_Color("Quest!!\n", ConsoleColor.Yellow);
            Console.WriteLine(quests[index].title); // 퀘스트 요약 타이틀
            Console.WriteLine(quests[index].info); // 퀘스트 설명
            Console.WriteLine(quests[index].goalInfo, quests[index].count, quests[index].goal); // 목표 설명                                                                
            quests[index].PrintRewards(); // 보상 설명
            Console.WriteLine();
            Console.Write("0. 나가기\n\n원하는 동작을 선택해주세요 >> ");
            while (true)
            {
                string next = Console.ReadLine();
                int next_int;
                if (!int.TryParse(next, out next_int))
                {
                    Console.Clear();
                    Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                    Console.ReadKey();
                }
                // 0번: 뒤로가기(퀘스트 목록 씬으로 이동)
                else if (next_int == 0)
                {
                    Console.Clear();
                    PrintQuestList();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("목록에 나온 숫자만 입력하세요.");
                    Console.ReadKey();
                }
            }
        }

        // 퀘스트 달성도 경신 후 달성 여부 체크
        // !!!!! 합친 이후에 구조 보고 각 작업자님께 말하고 추가할 것
        // 0: 전투 결과 때 몬스터 출현 수를 더하기
        // 1: 장비 장착할 때 +1
        // 2: 플레이어 레벨업 메서들 호출 때마다 +1
        public void QuestRenewal(int index, int _count)
        {
            quests[index].CheckAccomlish(_count);
        }

        // 다른 글자색을 표현하기 위한 메서드들
        public void WriteLine_Color(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color; // 앞으로 사용할 글자 색상 교체
            Console.WriteLine(text); // 바꾼 글자 색으로 출력하고 한줄 띄우기
            Console.ResetColor(); // 색상 원복
        }
        public void Write_Color(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color; // 앞으로 사용할 글자 색상 교체
            Console.Write(text); // 바꾼 글자 색으로 출력하고
            Console.ResetColor(); // 색상 원복
        }
    }

    struct QuestForm
    {
        public string
            title, // 퀘스트 명칭
            info,  // 퀘스트에 대한 상세한 설명
            goalInfo; // 퀘스트 목표

        public int 
            count, // 달성도
            goal; // 목표치

        public bool isAccomplish; // 퀘스트 달성 여부

        public Rewards rewards; // 보상

        // 달성량 추가 후 퀘스트 달성 여부 체크 >> 각 필요한 화면에서 불러오기
        public void CheckAccomlish(int addCount)
        { 
            if (isAccomplish) // 이미 달성한 퀘스트라면 메서드를 종료
                return;

            count += addCount; // 카운트 증가
            if(count >= goal) // 달성하지 않은 퀘스트의 목표를 달성했을 때 
            {  
                isAccomplish = true; // 달성 상태 기록
                Reward(); // 보상 지급
                AlarmAccomplish(); // 보상 지급창 팝업
            }
        }

        // 퀘스트 보상 받기
        public void Reward()
        {
            Program.player.gold += rewards.gold;    // 골드
            Program.player.exp += rewards.exp;      // 경험치
            // 경험치 올라갔을 때 레벨업이 되는지 체크 >> 만들어진 로직에 잇기 !!!!!

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

        // 임시 퀘스트 씬 : 퀘스트 완료, 보상 창을 보여주고 아무 키 입력으로 원래대로 돌아감
        // 퀘스트가 완료되면 즉시 나왔다가 키 입력으로 사라짐
        public void AlarmAccomplish()
        {
            // 새로운 콘솔 출력창(새 창을 띄우진 않으나 새 문서 같은 느낌으로 덮어써서 보이게끔)
            StringWriter stringWriter = new StringWriter();

            // 기존 콘솔 출력창
            TextWriter originalConsoleOut = Console.Out;

            // 새로운 콘솔 출력창 출현(비어 있음)
            Console.SetOut(stringWriter);

            // 퀘스트 달성 및 보상 알림
            Console.WriteLine("퀘스트 달성\n\n{0}\n\n", title);
            PrintRewards();

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

        // 퀘스트 보상 출력
        public void PrintRewards()
        {
            // 퀘스트 보상 골드, 경험치 출력
            Console.WriteLine("GOLD : {0} G\nEXP : {1} Exp\n\n", rewards.gold, rewards.exp);
            if (rewards.items != null)
            {
                // 퀘스트 보상 아이템들 목록 전체에 대해
                for (int i = 0; i < rewards.items.Length; i++)
                {
                    // 아이템 이름 x 갯수 출력
                    Console.WriteLine($"{rewards.items[i].item.itemName} x {rewards.items[i].amount}");
                }
                Console.WriteLine(); // 한줄 더 띄워서 아래와 붙지 않도록
            }
        }

        // 현재 보상은 크게 골드, 경험치, 아이템 셋으로 나뉩니다.
        // 아이템은 동시에 여러 종류를 줄 수 있게 이와 같이 만들었습니다.
        public struct Rewards
        {
            public int gold;
            public int exp;
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
