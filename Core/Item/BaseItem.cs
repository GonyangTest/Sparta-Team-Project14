using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpartaDungeon.Core.Item
{
    [Serializable]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ItemType
    {
        Weapon,
        Armor,
        Ring
    };

    [Serializable]
    public abstract class BaseItem : ICloneable
    {
        // 아이템 기본 정보
        [JsonProperty]
        protected int _id;
        [JsonProperty]
        protected string _name;
        [JsonProperty]
        protected string _description;
        [JsonProperty]
        protected int _price;
        [JsonProperty]
        protected int _isEquippable;
        [JsonProperty]
        protected ItemType _itemType;
        [JsonProperty]
        protected ItemOptions _options;

        // 옵션
        [JsonProperty]
        protected int _power;
        [JsonProperty]
        protected int _defense;

        public int Id { get { return _id; } set { _id = value; } }
        public int Power { get { return _power; } set { _power = value; } }
        public int Defense { get { return _defense; } set { _defense = value; } }
        public int Price { get { return _price; } }
        public ItemType ItemType { get { return _itemType; } }

        public ItemOptions Options {
            get { return _options; }
        }

        /// <summary>
        /// 아이템 복사
        /// </summary>
        /// <returns>아이템 복사</returns>
        public object Clone()
        {
            BaseItem clone = (BaseItem)MemberwiseClone();
            clone._options = (ItemOptions)_options.Clone();

            return clone;
        }

        /// <summary>
        /// 아이템 옵션 문자열 반환
        /// </summary>
        /// <returns>아이템 옵션 문자열</returns>
        public string GetOptionAsString() {
            return _options.ToString();
        }

        /// <summary>
        /// 아이템 생성자
        /// </summary>
        /// <param name="id">아이템 ID</param>
        /// <param name="name">아이템 이름</param>
        /// <param name="description">아이템 설명</param>
        /// <param name="value">아이템 가격</param>
        /// <param name="options">아이템 옵션</param>
        public BaseItem(int id, string name, string description, int value, ItemOptions options)
        {
            _id = id;
            _name = name;
            _description = description;
            _price = value;
            _options = options;
        }
        
        /// <summary>
        /// 아이템 문자열 반환
        /// </summary>
        /// <returns>아이템 문자열</returns>
        public override string ToString()
        {
            return $"{_name} | {GetOptionAsString()} | {_description}";
        }
    }
}
