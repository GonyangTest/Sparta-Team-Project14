using System.ComponentModel;

namespace SpartaDungeon.Core.Dungeon
{
    public enum DungeonType
    { 
        [Description("쉬운 던전")]
        Easy,
        [Description("일반 던전")]
        Normal,
        [Description("어려운 던전")]
        Hard
    }
} 