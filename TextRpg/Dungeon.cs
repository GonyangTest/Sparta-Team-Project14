using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        Console.WriteLine(player.CurrentPlayer());
                        Console.WriteLine("\n0. 나가기");
                        Console.ReadLine();
                        break;
                    case "2":
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
            Console.Clear();
            List<Stage> stages = new List<Stage>();
            try
            {
                Console.Clear();
                Console.WriteLine("Battle");
                /*for (int i=0; i<10 ; i++)
                {
                    switch (i)
                    {
                        case 1:
                            //1스테이지에선 몬스터 3마리 1층몬스터만 소환
                            break;
                        case 2:
                            //2스테이지에선 몬스터 3마리 2층몬스터까지 소환
                            break;
                        case 3:
                            //2스테이지에선 몬스터 3마리 3층몬스터까지 소환
                            break;
                        case 4:
                            //2스테이지에선 몬스터 3마리 4층몬스터까지 소환
                            break;
                        case 5:
                            //2스테이지에선 몬스터 3마리 5층몬스터까지 소환,보스몹 소환
                            break;
                        case 6:
                            //2스테이지에선 몬스터 3마리 6층몬스터까지 소환
                            break;
                        case 7:
                            //2스테이지에선 몬스터 3마리 7층몬스터까지 소환
                            break;
                        case 8:
                            //2스테이지에선 몬스터 3마리 8층몬스터까지 소환
                            break;
                        case 9:
                            //2스테이지에선 몬스터 3마리 9층몬스터까지 소환
                            break;
                        default:
                            //마지막 스테이지에선 몬스터 3마리 10층몬스터까지 소환,보스몹 소환
                            break;
                    }
                    //stages.Add(new Stage("1단계", normalCount: 2, eliteCount: 1, bossCount: 0));
                }*/
                var stage1 = new Stage("1단계", normalCount: 2, eliteCount: 1, bossCount: 0);
                var stage2 = new Stage("2단계", normalCount: 0, eliteCount: 1, bossCount: 1);

                stage1.Start(player);
                stage2.Start(player);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[에러 발생] {ex.Message}");
                Console.ReadKey();
            }

        }
        /*private Stage CreateStageByLevel(int level)
        {
            string name = $"{level}단계";
            int normal = 3;
            int elite = 0;
            int boss = 0;

            // 각 층수별로 몬스터 구성 설정
            switch (level)
            {
                case 1:
                    // 1층 몬스터만 (노말 3)
                    normal = 3;
                    elite = 0;
                    boss = 0;
                    break;

                case 2:
                case 3:
                case 4:
                    // 1~4층 몬스터 포함 (노말 2~3, 엘리트 1)
                    normal = 2;
                    elite = 1;
                    boss = 0;
                    break;

                case 5:
                    // 5층 보스 등장 (노말 2, 엘리트 1, 보스 1)
                    normal = 2;
                    elite = 1;
                    boss = 1;
                    break;

                case 6:
                case 7:
                case 8:
                case 9:
                    // 고층 몬스터 (엘리트 강화)
                    normal = 1;
                    elite = 2;
                    boss = 0;
                    break;

                default:
                    // 마지막 10층 (보스 등장)
                    normal = 1;
                    elite = 1;
                    boss = 1;
                    break;
            }

            return new Stage(name, normal, elite, boss);
        }*/

    }
    public class Stage
    {
        public string Name { get; private set; }//던전이름
        public List<Monster> Monsters { get; private set; }//몬스터 리스트
        public bool Cleared { get; private set; }//클리어유무판독

        private static bool monstersLoaded = false;//

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
                Monsters.Add(MonsterFactory.Create("1"));

            for (int i = 0; i < eliteCount; i++)
                Monsters.Add(MonsterFactory.Create("2"));

            for (int i = 0; i < bossCount; i++)
                Monsters.Add(MonsterFactory.Create("3"));

            Cleared = false;
        }

        internal void Start(Player player)
        {
            Console.WriteLine($"=== {Name} 스테이지 시작 ===");

            foreach (Monster monster in Monsters)
            {
                Console.WriteLine(monster);
                Battle(player, Monsters);
            }

            Cleared = true;
            Console.WriteLine($"=== {Name} 스테이지 클리어! ===\n");
            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();
        }


        private void Battle(Player player, List<Monster> monsters)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");

                // 몬스터 목록 출력
                for (int i = 0; i < monsters.Count; i++)
                {
                    var m = monsters[i];
                    string status = m.CurrentHP <= 0 ? "Dead" : $"HP {m.CurrentHP}";
                    Console.WriteLine($"{i + 1}. Lv.{m.Level} {m.Name}  {status}");
                }

                Console.WriteLine("\n[내정보]");
                Console.WriteLine($"Lv.{player.level} {player.playerName} ({player.playerClass})");
                Console.WriteLine($"HP {player.hp}/100\n");

                Console.WriteLine("0. 취소\n");
                Console.Write("대상을 선택해주세요.\n>> ");
                string input = Console.ReadLine();

                if (input == "0") return;

                if (int.TryParse(input, out int index) && index > 0 && index <= monsters.Count)
                {
                    var target = monsters[index - 1];

                    if (target.CurrentHP <= 0)
                    {
                        Console.WriteLine("이미 죽은 몬스터입니다.");
                        Console.ReadKey();
                        continue;
                    }

                    // 공격 처리
                    Console.Clear();
                    Console.WriteLine("Battle!!\n");
                    int damage = Math.Max(0, player.totalPower - target.Defense);
                    target.CurrentHP -= damage;

                    Console.WriteLine($"{player.playerName} 의 공격!");
                    Console.WriteLine($"Lv.{target.Level} {target.Name} 을(를) 맞혔습니다. [데미지 : {damage}]");

                    if (target.CurrentHP <= 0)
                        Console.WriteLine($"\nLv.{target.Level} {target.Name}\nHP {target.MaxHP} -> Dead");
                    else
                        Console.WriteLine($"\nLv.{target.Level} {target.Name}\nHP {target.CurrentHP}");

                    Console.WriteLine("\n0. 다음\n>>");
                    Console.ReadLine();

                    // 살아있는 몬스터들의 반격
                    foreach (var m in monsters)
                    {
                        if (m.CurrentHP > 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Battle!!\n");

                            int retaliation = Math.Max(0, m.Attack - player.totalDefense);
                            player.hp -= retaliation;

                            Console.WriteLine($"Lv.{m.Level} {m.Name} 의 공격!");
                            Console.WriteLine($"{player.playerName} 을(를) 맞혔습니다. [데미지 : {retaliation}]");
                            Console.WriteLine($"\nLv.{player.level} {player.playerName}\nHP {player.hp + retaliation} -> {player.hp}");

                            Console.WriteLine("\n0. 다음\n>>");
                            Console.ReadLine();

                            if (player.hp <= 0)
                                break;
                        }
                    }

                    if (player.hp <= 0 || monsters.All(m => m.CurrentHP <= 0))
                        break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }

            Console.Clear();
            Console.WriteLine("Battle!! = Result\n");

            if (player.hp > 0)
            {
                Console.WriteLine("Victory\n");
                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");
            }
            else
            {
                Console.WriteLine("You Lose\n");
            }

            Console.WriteLine($"\nLv.{player.level} {player.playerName}\nHP {player.hp}");
            Console.WriteLine("\n0. 다음\n>>");
            Console.ReadLine();
        }

    }
}
