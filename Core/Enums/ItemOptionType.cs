using System.ComponentModel;
using Newtonsoft.Json;
using System;

namespace SpartaDungeon.Core.Enums
{
    [Serializable]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ItemOptionType
    {
        [Description("공격력")]
        Power,
        [Description("방어력")]
        Defense,
        [Description("체력")]
        Health,
    }
}
