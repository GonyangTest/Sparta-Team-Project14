using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Monster
    { // 몬스터의 기본 스텟 받아오기
        private int _stage;
        private string _name;
        private int _level;
        private int _maxHP;
        private double _currentHP;
        private int _attack;
        private int _defense;
        private int _dropExp;
        private int _dropGold;
        private string _art; // 몬스터 아스키 아트 저장

        public int Stage { get => _stage; }
        public string Name { get => _name; }
        public int Level { get => _level; }
        public int MaxHP { get => _maxHP; }
        public double CurrentHP { get => _currentHP; set => _currentHP = Math.Max(0, Math.Min(value, _maxHP));}
        public int Attack { get => _attack; }
        public int Defense { get => _defense; }
        public int DropExp { get => _dropExp; }
        public int DropGold { get => _dropGold; }
        public bool IsAlive => _currentHP > 0;

        // 생성자 추가
        public Monster()
        {
            // 기본 생성자
        }

        // 편의를 위한 매개변수 있는 생성자
        public Monster(int stage, string name, int level, int maxHP, int attack, int defense, int dropExp, int dropGold)
        {
            _stage = stage;
            _name = name;
            _level = level;
            _maxHP = Math.Max(1, maxHP);
            _attack = Math.Max(0, attack);
            _defense = Math.Max(0, defense);
            _dropExp = Math.Max(0, dropExp);
            _dropGold = Math.Max(0, dropGold);
            _currentHP = _maxHP;
        }

        // 아스키 아트 설정 메서드
        public void SetArt(string art)
        {
            _art = art;
        }

        public override string ToString()
        { // 몬스터가 생성되었을때 나오는 UI
            return $"Lv.{Level} {Name} HP: {CurrentHP}/{MaxHP}";
        }

        public void Hit(double damage)
        {
            _currentHP = Math.Max(0, _currentHP - damage);
        }
    }
}
