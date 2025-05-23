﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TextRpg
{
    public class Item
    {
        // 상속개념 파악하기
        // 아이템 생성자 생성했으니까 출력하고 공격무기 방어무기 나눠 (상속)
        protected string _itemName ;
        protected string _description;
        protected int _price;
        protected bool _isPurchased;

        public string ItemName
        {
            get { return _itemName; }
        }

        public string Description
        {
            get { return _description; }
        }
        public int Price
        {
            get { return _price; }
        }

        public bool IsPurchased
        {
            get { return _isPurchased; }
            set { _isPurchased = value; }
        }


        public Item(string name, string desc, int pri, bool isPurchased = false)
        {// 아이템에는 무기와 방어구가 있으므로 공통된 내용인 이름 설명 가격 구매여부를 인자값으로 생성자를 만든다.
            //초기화
            _itemName = name;
            _description = desc;
            _price = pri;
            this._isPurchased = isPurchased;
        }
        public virtual string GetInfo()
        {// 상점창에서 아이템을 나타내는 양식, '?'는 삼항 연산자를 사용한 조건문으로 true면 "구매완료", false면 가격을 표시
         // 삼항연산자 조건문(bool변수 ? true일때 : false일때)

            return $" {_itemName} | {_description} | {(IsPurchased ? "구매완료" : $"{Price} G")}";
        }

    }
}
