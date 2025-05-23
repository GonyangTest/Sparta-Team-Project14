﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Weapon : Item
    {// Item을 상속받는 상속클래스
        public int _attack;

        public int Attack
        {
            get { return _attack; }
        }

        public Weapon(string name, int atk, string desc, int pri, bool isPurchased = false)
            : base(name, desc, pri, isPurchased)
        {
            _attack = atk;
        }
        public override string GetInfo()
        {// 상속받는 클래스인 Item의 메서드에서 수정하고싶은 부분이 생겼을때 override를 사용해 메서드를 재정의한다.
            return $" {_itemName} | 공격력 +{_attack} | {_description} | {(_isPurchased ? "구매완료" : $"{_price} G")}";
        }
    }
}
