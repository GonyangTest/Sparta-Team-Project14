using System.ComponentModel;
using System.Reflection;
using System;
using Newtonsoft.Json;

namespace SpartaDungeon.Core.Enums {
    [Serializable]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ClassType
    {
        [Description("전사")]
        Warrior,
        [Description("마법사")]
        Mage,
        [Description("궁수")]
        Archer,
    }  
}
