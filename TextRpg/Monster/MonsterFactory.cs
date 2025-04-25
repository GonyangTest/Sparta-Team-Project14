using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextRpg
{
    public static class MonsterFactory
    {
        // 몬스터 데이터베이스 (ID를 키로 사용)
        private static Dictionary<int, Monster> _monsterDatabase = new Dictionary<int, Monster>();

        static MonsterFactory()
        {
            LoadMonsters(GameConstance.Dungeon.MONSTER_CSV_PATH);
        }
        
        // CSV 파일에서 몬스터 정보를 읽어들여 데이터베이스로 저장하는 메서드
        private static void LoadMonsters(string filePath)
        {
            // 파일이 존재하지 않으면 오류 메시지 출력 후 종료
            if (!File.Exists(filePath))
            {
                Console.WriteLine("CSV 파일을 찾을 수 없습니다: " + filePath);
                return;
            }

            // 모든 줄을 한 번에 읽음 (첫 줄은 헤더)
            var lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++) // 첫 번째 줄은 헤더라서 건너뜀
            {
                var tokens = lines[i].Split(','); // 콤마(,)로 구분된 값 분리

                if (tokens.Length < 8) continue;  // 데이터가 부족하면 스킵

                // 몬스터 객체 생성 및 CSV에서 값 파싱
                var monster = new Monster
                (
                    stage: int.Parse(tokens[0]),
                    name: tokens[1],
                    level: int.Parse(tokens[2]),
                    maxHP: int.Parse(tokens[3]),
                    attack: int.Parse(tokens[4]),
                    defense: int.Parse(tokens[5]),
                    dropExp: int.Parse(tokens[6]),
                    dropGold: int.Parse(tokens[7])
                )
                {
                };

                // 타입을 키로 해서 몬스터 템플릿 저장 (덮어쓰기 가능)
                _monsterDatabase[monster.Stage] = monster;
            }
        }
        
        // ID로 몬스터 가져오기
        public static Monster GetMonster(int id)
        {
            if (_monsterDatabase.TryGetValue(id, out Monster monster))
            {
                // 새로운 Monster 인스턴스를 생성자를 활용하여 복사본 생성
                return new Monster(
                    stage: monster.Stage,
                    name: monster.Name,
                    level: monster.Level,
                    maxHP: monster.MaxHP,
                    attack: monster.Attack,
                    defense: monster.Defense,
                    dropExp: monster.DropExp,
                    dropGold: monster.DropGold
                );
            }
            return null;
        }
        
        // 스테이지별 몬스터 목록 가져오기
        public static Monster GetMonsterByStage(int stage)
        {
            Monster monster = new Monster(
                stage: stage,
                name: _monsterDatabase[stage].Name,
                level: _monsterDatabase[stage].Level,
                maxHP: _monsterDatabase[stage].MaxHP,
                attack: _monsterDatabase[stage].Attack,
                defense: _monsterDatabase[stage].Defense,
                dropExp: _monsterDatabase[stage].DropExp,
                dropGold: _monsterDatabase[stage].DropGold
            );
            return monster;
        }
        
        // 지정한 스테이지의 몬스터를 생성
        public static List<Monster> Create(int stage)
        {
            List<Monster> stageMonsters = new List<Monster>();
            // 총 1~4 몬스터 출력
            // 스테이지 증가 할때마다 스테이지 몬스터 추가
            Random random = new Random();
            int monsterCount = random.Next(1, GameConstance.Monster.MAX_MONSTER_COUNT + 1); // 1~4 몬스터 출력
            for (int i = 0; i < monsterCount; i++)
            {
                if (stage > 3){
                    stageMonsters.Add(GetMonsterByStage(random.Next(stage-1, stage + 1))); // 10 스테이지 인경우 1~10 몬스터 출력
                }
                else{
                    stageMonsters.Add(GetMonsterByStage(random.Next(1, stage + 1))); // 10 스테이지 인경우 1~10 몬스터 출력
                }
                
            }
            return stageMonsters;
        }
        
        // 전체 몬스터 목록 반환
        public static List<Monster> GetAllMonsters()
        {
            return _monsterDatabase.Values
                .Select(monster => new Monster(
                    stage: monster.Stage,
                    name: monster.Name,
                    level: monster.Level,
                    maxHP: monster.MaxHP,
                    attack: monster.Attack,
                    defense: monster.Defense,
                    dropExp: monster.DropExp,
                    dropGold: monster.DropGold
                ))
                .ToList();
        }
    }
}
