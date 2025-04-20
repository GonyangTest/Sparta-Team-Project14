using SpartaDungeon.Core;
using SpartaDungeon.User;
using SpartaDungeon.Core.Constants;
using SpartaDungeon.Core.Data;
using SpartaDungeon.Core.Dungeon;
using SpartaDungeon.Core.Dungeon.Interface;
using SpartaDungeon.Core.Item;
using SpartaDungeon.Core.UI.Scene;
using SpartaDungeon.Core.Equipment.Interface;
using SpartaDungeon.Core.Inventory.Interface;
using SpartaDungeon.Core.Shop;
using SpartaDungeon.Core.Shop.Interface;
using System.Text;

namespace SpartaDungeon.Managers
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    public class GameManager
    {
        public static GameManager? _Instance = null;
        private Player? _player = null;
        private IItemShop? _shop = null;
        private bool _isGameRunning = false;
        BaseScene? _currentScene = null;
        DungeonResult? _dungeonResult = null;
        
        // DungeonManager 추가
        private IDungeonManager? _dungeonManager = null;

        public static GameManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameManager();
                }
                return _Instance;
            }
        }

        public DungeonResult? DungeonResult
        {
            get { return _dungeonResult; }
        }

        public void StartGame()
        {
            // 씬 로드
            LoadScene();

            // 게임 시작
            GameLoop();
        }

        /// <summary>
        /// 새로운 게임 시작
        /// </summary>
        public void NewGame()
        {
            IPlayerInventory inventory = DependencyContainer.CreateInventory();
            IPlayerEquipmentSlot equipmentSlot = DependencyContainer.CreateEquipmentSlot();
            
            _player = DependencyContainer.CreatePlayer(inventory, equipmentSlot);
            _shop = DependencyContainer.CreateShop();

            // 플레이어 초기화
            _player.Init();
            _shop.Init();
            
            _dungeonManager = DependencyContainer.CreateDungeonManager(_player);
        }

        /// <summary>
        /// 게임 로드
        /// </summary>
        /// <returns>게임 로드 성공 여부</returns>
        public bool LoadGame()
        {
            _player = SaveManager.instance.LoadPlayerData();
            _shop = SaveManager.instance.LoadShopData();
            
            if (_player != null)
            {
                _dungeonManager = DependencyContainer.CreateDungeonManager(_player);
                return _shop != null;
            }
            
            return false;
        }

        /// <summary>
        /// 게임 저장
        /// </summary>
        /// <returns>게임 저장 성공 여부</returns>
        public bool SaveGame()
        {
            if (_player == null || _shop == null)
            {
                Console.WriteLine("플레이어나 상점 정보가 없습니다.");
                return false;
            }

            SaveManager.instance.SavePlayerData(_player);
            SaveManager.instance.SaveShopData(_shop);
            return true;    
        }

        /// <summary>
        /// 플레이어 상태 반환
        /// </summary>
        /// <returns>플레이어 상태 문자열</returns>
        public string GetPlayerStatus()
        {
            if(_player == null)
            {
                Console.WriteLine("플레이어 정보가 없습니다.");
                return "";
            }

            return _player.GetStatusAsString();
        }

        /// <summary>
        /// 아이템 장착 해제
        /// </summary>
        /// <param name="itemType">아이템 타입</param>
        public void UnEquipItem(ItemType itemType)
        {
            _player?.UnEquipItem(itemType);
        }

        /// <summary>
        /// 아이템 장착
        /// </summary>
        /// <param name="index">아이템 인덱스</param>
        public void EquipItem(int index)
        {
            _player?.EquipItem(index);
        }

        /// <summary>
        /// 게임 메인 루프
        /// </summary>
        private void GameLoop()
        {
            _isGameRunning = true;
            SceneManager.Instance.ChangeScene(SceneType.Intro);

            while (_isGameRunning)
            {
                // 현재 씬 업데이트
                BaseScene? newScene = SceneManager.Instance.GetCurrentScene();
                if (newScene == null)
                {
                    _isGameRunning = false;
                    return;
                }

                if (_currentScene != newScene)
                {
                    _currentScene = newScene;
                    _currentScene.Update();
                }

                // 사용자 입력 처리
                ProcessInput();
            }
        }

        /// <summary>
        /// 사용자 입력 처리
        /// </summary>
        private void ProcessInput()
        {
            BaseScene? currentScene = SceneManager.Instance.GetCurrentScene();
            if (currentScene == null) return;

            Console.WriteLine(Global.INPUT_PROMPT);
            string? input = Console.ReadLine();
            
            if (string.IsNullOrEmpty(input))
            {
                currentScene.Update();
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            // 현재 씬에 입력 전달
            currentScene.HandleInput(input);
        }

        /// <summary>
        /// 플레이어 골드 반환
        /// </summary>
        /// <returns>플레이어 골드</returns>
        public string GetPlayerGold()
        {
            if (_player == null)
            {
                Console.WriteLine("플레이어 정보가 없습니다.");
                return "0";
            }
            return _player.PlayerStatus.Gold.ToString();
        }

        /// <summary>
        /// 플레이어 인벤토리 아이템 반환
        /// </summary>
        /// <returns>플레이어 인벤토리 아이템</returns>
        public List<BaseItem> GetInventoryItmes()
        {
            if (_player == null)
            {
                Console.WriteLine("플레이어 정보가 없습니다.");
                return new List<BaseItem>();
            }
            return _player.Inventory.GetItemList();
        }

        /// <summary>
        /// 플레이어 인벤토리 아이템 문자열 반환
        /// </summary>
        /// <param name="isEquipping">아이템 장착 여부</param>
        /// <returns>플레이어 인벤토리 아이템 문자열</returns>
        public string GetInventoryAsString(bool isEquipping)
        {
            if(_player == null)
            {
                Console.WriteLine("플레이어 정보가 없습니다.");
                return "";
            }
            
            List<BaseItem> items = _player.Inventory.GetItemList();
            if (items.Count == 0)
            {
                return "";
            }
            
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < items.Count; i++)
            {
                sb.Append("- ");
                
                if (isEquipping == true)
                {
                    sb.Append($"{i + 1 + _player.EquipmentSlot.GetItemCount()}. ");
                }

                sb.Append($"{items[i].ToString()}");
                
                if (i != items.Count - 1)
                {
                    sb.Append("\n");
                }
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 플레이어 장착 아이템 반환
        /// </summary>
        /// <returns>플레이어 장착 아이템</returns>
        public List<BaseItem> GetEquipmentItmes()
        {
            if (_player == null)
            {
                Console.WriteLine("플레이어 정보가 없습니다.");
                return new List<BaseItem>();
            }
            return _player.EquipmentSlot.GetItemList();
        }

        /// <summary>
        /// 플레이어 장착 아이템 문자열 반환
        /// </summary>
        /// <param name="isEquipping">아이템 장착 여부</param>
        /// <returns>플레이어 장착 아이템 문자열</returns>
        public string GetEquipmentAsString(bool isEquipping)
        {
            if (_player == null)
            {
                Console.WriteLine("플레이어 정보가 없습니다.");
                return "";
            }
            
            List<BaseItem> items = _player.EquipmentSlot.GetItemList();
            if (items.Count == 0)
            {
                return "";
            }
            
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < items.Count; i++)
            {
                sb.Append("- ");
                
                if (isEquipping == true)
                {
                    sb.Append($"{i + 1}. ");
                }

                sb.Append($"[E]{items[i].ToString()}");
                
                if(i != items.Count - 1)
                {
                    sb.Append("\n");
                }
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 상점 아이템 반환
        /// </summary>
        /// <returns>상점 아이템</returns>
        public List<BaseItem> GetShopItems()
        {
            if (_shop == null)
            {
                Console.WriteLine("상점 정보가 없습니다.");
                return new List<BaseItem>();
            }
            return _shop.GetShopItemList();
        }

        /// <summary>
        /// 상점 아이템 문자열 반환
        /// </summary>
        /// <param name="isBuying">구매 여부</param>
        /// <returns>상점 아이템 문자열</returns>
        public string GetShopItemsAsString(bool isBuying)
        {
            if (_shop == null)
            {
                Console.WriteLine("상점 정보가 없습니다.");
                return "";
            }
            
            List<BaseItem> items = _shop.GetShopItemList();
            if (items.Count == 0)
            {
                return "";
            }
            
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < items.Count; i++)
            {
                sb.Append("- ");
                
                if (isBuying == true)
                {
                    sb.Append($"{i + 1}. ");
                }

                if (_shop.HasStock(i) == false)
                {
                    sb.Append($"{items[i].ToString()} | 구매완료");
                }
                else
                {
                    sb.Append($"{items[i].ToString()} | {items[i].Price}");
                }
                
                if (i != items.Count - 1)
                {
                    sb.Append("\n");
                }
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 아이템 구매
        /// </summary>
        /// <param name="index">상점 아이템 목록 인덱스</param>
        /// <returns>아이템 구매 결과</returns>
        public string BuyItem(int index)
        {
            if (_shop == null || _player == null)
            {
                
                return "상점 또는 플레이어 정보가 없습니다.";
            }

            if(_shop.HasStock(index))
            {
                BaseItem? item = _shop.GetItem(index);
                if(item == null)
                {
                    return "잘못된 인덱스입니다.";
                }
                BaseItem clone = ItemDataBase.Instance.CreateItem(item.Id);
                if (_player.BuyItem(clone))
                {
                    _shop.DecreaseStock(index);
                }
                else
                {
                    return "골드가 부족합니다.";
                }
            }
            else
            {
                return "이미 구매한 아이템입니다.";
            }

            return "구매를 완료했습니다.";
        }

        /// <summary>
        /// 아이템 판매
        /// </summary>
        /// <param name="index">인벤토리 목록 인덱스</param>
        /// <param name="item">아이템</param>
        /// <param name="isEquipped">아이템 장착 여부</param>
        /// <returns>아이템 판매 결과</returns>
        public string SellItem(int index, BaseItem item, bool isEquipped)
        {
            if (_shop == null || _player == null)
            {

                return "상점 또는 플레이어 정보가 없습니다.";
            }

            BaseItem? removedItem = null;

            if (isEquipped)
            {
                removedItem = _player.EquipmentSlot.RemoveItem(item.ItemType);
            }
            else
            {
                removedItem = _player.Inventory.RemoveItem(index);
            }

            if (removedItem == null)
            {
                return "잘못된 인덱스입니다.";
            }

            _player.AddGold((int)(removedItem.Price * 0.85));
            _shop.IncreaseStock(removedItem);
            return "판매를 완료했습니다.";
        }

        /// <summary>
        /// 휴식하여 체력을 회복
        /// </summary>
        /// <returns>휴식 결과</returns>
        public string Rest()
        {
            if (_player == null)
            {
                return "플레이어 정보가 없습니다.";
            }
            if (_player.PlayerStatus.Gold < Global.Rest.REST_COST)
            {
                return "골드가 부족합니다.";
            }
            _player.AddGold(-Global.Rest.REST_COST);
            _player.Recover();
            return "휴식이 완료되었습니다.";
        }

        /// <summary>
        /// 던전 입장
        /// </summary>
        /// <param name="level">던전 난이도</param>
        public void EnterDungeon(DungeonType level)
        {
            if(_player == null || _dungeonManager == null)
            {
                Console.WriteLine("플레이어 정보가 없습니다.");
                return;
            }

            // DungeonManager를 사용하여 던전 입장
            _dungeonManager.EnterDungeon(level);
            
            // 던전 결과 저장
            _dungeonResult = _dungeonManager.GetDungeonResult();
        }

        /// <summary>
        /// 씬 로드 및 등록
        /// </summary>
        private void LoadScene()
        {
            SceneManager.Instance.LoadAllScene();
        }
    }
}
