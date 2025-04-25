using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Armor : Item
    {// Item을 상속받는 상속클래스
        public int defense;

        public Armor(string name, int def, string desc, int pri, bool isPurchased = false) // 생성자를 만들고 아이템 정보를 보여주기위해 매개변수를 생성한다.
            : base(name, desc, pri, isPurchased) // base는 부모클래스의 생성자를 호출하는 구문이다.
                                                // base( name desc pri isPurshased ) 는 현재 생성자의 인자를 초기화해준다.
        {
            defense = def; // 부모클래스에 def인자가 없으므로 따로 초기화를 해준다.
        }

        public override string GetInfo()
        {// 상속받는 클래스인 Item의 메서드에서 수정하고싶은 부분이 생겼을때 override를 사용해 메서드를 재정의한다.
            return $" {itemName} | 방어력 +{defense} | {description} | {(isPurchased ? "구매완료" : $"{price} G")}";
        }
    }
}
