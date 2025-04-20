using System.Text.Json.Serialization;

namespace SpartaDungeon.Core.Dungeon
{
    [Serializable]
    public class DungeonResult
    {
        [JsonInclude]
        public DungeonType Level { get; set; }
        
        [JsonInclude]
        public int Reward { get; set; }
        
        [JsonInclude]
        public bool IsClear { get; set; }
        
        [JsonInclude]
        public int PreviousGold { get; set; }
        
        [JsonInclude]
        public int PreviousHealth { get; set; }
        
        [JsonInclude]
        public int HealthLost { get; set; }
    }
} 