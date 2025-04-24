using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Monster
    { // 몬스터의 기본 스텟 받아오기
        public string Stage { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int DropExp { get; set; }
        public int DropGold { get; set; }

        public override string ToString()
        { // 몬스터가 생성되었을때 나오는 UI
            return $"Lv.{Level} {Name} HP: {CurrentHP}/{MaxHP}";
        }
    }
}
