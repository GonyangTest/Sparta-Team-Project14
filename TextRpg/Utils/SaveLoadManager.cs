using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace TextRpg
{
    internal class SaveLoadManager
    {
        private const string SAVE_FILE_PATH = "save_data.json";

        // 게임 데이터 저장
        public static void SaveGame(Player player, Inventory inventory, Quest quest, Shop shop)
        {
            try
            {
                GameData gameData = new GameData();

                // 플레이어 데이터 저장
                gameData.PlayerData.FromPlayer(player);

                // 인벤토리 데이터 저장
                gameData.InventoryData.FromInventory(inventory);

                // 장착 아이템 인덱스 저장
                if (player.EquippedWeapon != null)
                {
                    gameData.PlayerData.EquippedWeaponIndex = inventory.inventory.IndexOf(player.EquippedWeapon);
                }

                if (player.EquippedArmor != null)
                {
                    gameData.PlayerData.EquippedArmorIndex = inventory.inventory.IndexOf(player.EquippedArmor);
                }

                // 퀘스트 데이터 저장
                gameData.QuestData.FromQuest(quest);

                // Shop의 아이템들을 저장
                gameData.ShopData.FromShop(shop);

                // JSON 직렬화 설정
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                // JSON 문자열 생성 및 파일 저장
                string jsonString = JsonSerializer.Serialize(gameData, options);
                File.WriteAllText(SAVE_FILE_PATH, jsonString);

                Console.WriteLine("게임이 성공적으로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 저장 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        // 게임 데이터 불러오기
        public static bool LoadGame(Player player, Inventory inventory, Quest quest, Shop shop)
        {
            if (!File.Exists(SAVE_FILE_PATH))
            {
                Console.WriteLine("저장된 게임 파일이 없습니다.");
                return false;
            }

            try
            {
                // 파일에서 JSON 문자열 읽기
                string jsonString = File.ReadAllText(SAVE_FILE_PATH);

                // JSON 역직렬화
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                GameData gameData = JsonSerializer.Deserialize<GameData>(jsonString, options);

                // 인벤토리 데이터 적용 (장착 아이템 참조를 위해 먼저 처리)
                gameData.InventoryData.ToInventory(inventory);

                // 플레이어 데이터 적용
                gameData.PlayerData.ToPlayer(player);

                // 장착 아이템 설정
                if (gameData.PlayerData.EquippedWeaponIndex >= 0 &&
                    gameData.PlayerData.EquippedWeaponIndex < inventory.inventory.Count)
                {
                    player.EquippedWeapon = inventory.inventory[gameData.PlayerData.EquippedWeaponIndex] as Weapon;
                }

                if (gameData.PlayerData.EquippedArmorIndex >= 0 &&
                    gameData.PlayerData.EquippedArmorIndex < inventory.inventory.Count)
                {
                    player.EquippedArmor = inventory.inventory[gameData.PlayerData.EquippedArmorIndex] as Armor;
                }

                // 퀘스트 데이터 적용
                gameData.QuestData.ToQuest(quest);
                quest.LoadQuestData();

                // 상점 데이터 적용
                gameData.ShopData.ToShop(shop);

                Console.WriteLine($"{player.playerName}의 게임 데이터를 성공적으로 불러왔습니다.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 불러오기 중 오류가 발생했습니다: {ex.Message}");
                return false;
            }
        }

        // 저장 파일 존재 여부 확인
        public static bool SaveFileExists()
        {
            return File.Exists(SAVE_FILE_PATH);
        }
    }
}
