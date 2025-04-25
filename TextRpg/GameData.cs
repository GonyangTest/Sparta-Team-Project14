using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    [Serializable]
    internal class GameData
    {
        // 플레이어 정보
        public PlayerData PlayerData { get; set; }

        // 인벤토리 정보
        public InventoryData InventoryData { get; set; }

        // 퀘스트 정보
        public QuestData QuestData { get; set; }

        public GameData()
        {
            PlayerData = new PlayerData();
            InventoryData = new InventoryData();
            QuestData = new QuestData();
        }
    }
}

