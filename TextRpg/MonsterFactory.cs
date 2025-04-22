using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public static class MonsterFactory
    {
        private static Dictionary<string, Monster> _monsterTemplates = new();

        public static void LoadMonsters(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("CSV 파일을 찾을 수 없습니다: " + filePath);
                return;
            }

            var lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
            {
                var tokens = lines[i].Split(',');

                if (tokens.Length < 6) continue;

                var monster = new Monster
                {
                    Type = tokens[0],
                    Name = tokens[1],
                    MaxHP = int.Parse(tokens[2]),
                    CurrentHP = int.Parse(tokens[2]), // 시작 체력 = 최대 체력
                    Attack = int.Parse(tokens[3]),
                    Defense = int.Parse(tokens[4]),
                    Speed = int.Parse(tokens[5])
                };

                _monsterTemplates[monster.Type] = monster;
            }
        }

        public static Monster Create(string type)
        {
            if (_monsterTemplates.TryGetValue(type, out var template))
            {
                return new Monster
                {
                    Type = template.Type,
                    Name = template.Name,
                    MaxHP = template.MaxHP,
                    CurrentHP = template.MaxHP,
                    Attack = template.Attack,
                    Defense = template.Defense,
                    Speed = template.Speed
                };
            }

            throw new ArgumentException($"'{type}' 몬스터는 존재하지 않습니다.");
        }
    }
    // **몬스터 생성 (던전 입장후 몬스터를 생성하면 됨**
    //string filePath = @"E:\TextRpg\TextRpg\monsters.csv";  // 경로 수정해야함!
    //MonsterFactory.LoadMonsters(filePath); 

    //try
    //{
    //    // 몬스터 생성 테스트
    //    var goblin = MonsterFactory.Create("Goblin");
    //    var troll = MonsterFactory.Create("Troll");

    //    Console.WriteLine(goblin);
    //    Console.WriteLine(troll);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine("에러 발생: " + ex.Message);
    //}
}
