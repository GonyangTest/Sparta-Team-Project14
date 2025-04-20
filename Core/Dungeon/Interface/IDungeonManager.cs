using SpartaDungeon.Core.Dungeon;
using SpartaDungeon.User;

namespace SpartaDungeon.Core.Dungeon.Interface
{
    public interface IDungeonManager
    {
        void EnterDungeon(DungeonType level);
        
        DungeonResult? GetDungeonResult();
    }
} 