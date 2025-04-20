using System.Collections;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using SpartaDungeon.Core;
using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Utils;

namespace SpartaDungeon.Core.Item
{
    /// <summary>
    /// 아이템 옵션 리스트
    /// </summary>
    [Serializable]
    public class ItemOptions : ICloneable
    {
        public object Clone()
        {
            ItemOptions clone = new ItemOptions();
            foreach (ItemOption option in _itemOptions)
            {
                clone.AddOption(option.Type, option.Value);
            }
            return clone;
        }

        [JsonProperty]
        private List<ItemOption> _itemOptions = new List<ItemOption>(); // 아이템 옵션 리스트
        public ItemOptions() { }

        /// <summary>
        /// 아이템 옵션 추가
        /// </summary>
        /// <param name="itemOptionType">아이템 옵션 타입</param>
        /// <param name="value">아이템 옵션 값</param>
        public void AddOption(ItemOptionType itemOptionType, float value)
        {
            _itemOptions.Add(new ItemOption(itemOptionType, value));
        }

        /// <summary>
        /// 아이템 옵션 추가
        /// </summary>
        /// <param name="itemOptionType">아이템 옵션 타입</param>
        /// <param name="value">아이템 옵션 값</param>
        public void AddOption(ItemOptionType itemOptionType, int value)
        {
            _itemOptions.Add(new ItemOption(itemOptionType, value));
        }

        public List<ItemOption> Get()
        {
            return _itemOptions;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _itemOptions.Count; i++)
            {
                ItemOption option = _itemOptions[i];
                if(option.Value != 0)
                    sb.Append(option.ToString() + (i == _itemOptions.Count - 1 ? "" : ","));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 아이템 옵션
    /// </summary>
    [Serializable]
    public class ItemOption
    {
        [JsonProperty]
        public ItemOptionType Type { get; private set; }
        [JsonProperty]
        public float Value { get; private set; }

        public ItemOption(ItemOptionType type, float value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{EnumUtils.GetDescription(Type)} +{Value}";
        }
    }
}
