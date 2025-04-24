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
            int[] floor = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            try
            {
                Console.Clear();
                Console.WriteLine("Battle");

                for(int i = 1; i<=floor.Length; i++)
                {
                    stages.Add(new Stage($"{i}단계",normalCount: 1, eliteCount: 1, bossCount: 1));
                }

                foreach(Stage stage in stages)
                {
                    stage.Start(player);
                    if (player.hp <= 0)
                    {
                        Console.WriteLine("플레이어가 사망했습니다. 던전에서 퇴장합니다...");
                        Thread.Sleep(2000);
                        break;
                    }

                    if (!stage.Cleared)
                    {
                        Console.WriteLine("스테이지 클리어에 실패했습니다...");
                        Thread.Sleep(2000);
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
        }

        internal void Start(Player player)
        {
            Console.WriteLine($"=== {Name} 스테이지 시작 ===");

            foreach (Monster monster in Monsters)
            {
                Console.WriteLine(monster);
                
            }

            Battle(player, Monsters);

            if(player.hp > 0 && Monsters.All(m => m.CurrentHP <= 0))
            {
                Cleared = true;
                Console.WriteLine($"=== {Name} 스테이지 클리어! ===\n");
            }

            
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
                    //int damage = (int)(player.totalPower * (100f / (100 + target.Defense)));
                    //damage = Math.Max(3, damage); //-> 데미지가 너무약하면 아래 코드 주석처리하고 이코드 풀어서 사용하세요

                    int damage = Math.Max(1, (int)(player.totalPower - target.Defense));
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

                            int retaliation = Math.Max(1, m.Attack - player.totalDefense);
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
