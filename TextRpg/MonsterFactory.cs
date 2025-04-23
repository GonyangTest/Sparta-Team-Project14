using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public static class MonsterFactory
    {
        // 몬스터 타입별로 기본 설정(템플릿)을 저장하는 딕셔너리
        private static Dictionary<string, Monster> _monsterTemplates = new();

        // CSV 파일에서 몬스터 정보를 읽어들여 템플릿으로 저장하는 메서드
        public static void LoadMonsters(string filePath)
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
                    stage: tokens[0],
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
                _monsterTemplates[monster.Stage] = monster;
            }
        }

        // 지정한 타입의 몬스터를 새로 생성 (템플릿을 기반으로 복사)
        public static Monster Create(string type)
        {
            if (_monsterTemplates.TryGetValue(type, out var template))
            {
                // 새로운 Monster 인스턴스를 생성자를 활용하여 생성
                var monster = new Monster(
                    stage: template.Stage,
                    name: template.Name,
                    level: template.Level,
                    maxHP: template.MaxHP,
                    attack: template.Attack,
                    defense: template.Defense,
                    dropExp: template.DropExp,
                    dropGold: template.DropGold
                )
                {
                };
                
                return monster;
            }

            // 존재하지 않는 타입 요청 시 예외 발생
            throw new ArgumentException($"'{type}' 몬스터는 존재하지 않습니다.");
        }
    }
}
