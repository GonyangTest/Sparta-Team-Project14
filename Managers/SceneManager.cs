using System.ComponentModel;
using SpartaDungeon.Core.UI.Scene;

namespace SpartaDungeon.Managers
{
    public enum SceneType
    {
        [Description("시작화면")]
        Intro,
        [Description("메인메뉴")]
        MainMenu,
        [Description("인벤토리")]
        Inventory,
        [Description("상점")]
        Shop,
        [Description("구매")]
        Buy,
        [Description("판매")]
        Sell,
        [Description("상태보기")]
        Status,
        [Description("장비관리")]
        Equipment,
        [Description("던전입장")]
        Dungeon,
        [Description("휴식하기")]
        Rest,
        [Description("던전클리어")]
        DungeonClear,

    }
    class SceneManager
    {
        private static SceneManager? _instance = null;
        private BaseScene? _currentScene = null;
        private Dictionary<SceneType, BaseScene> _sceneDictionary = new Dictionary<SceneType, BaseScene>();

        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneManager();
                }
                return _instance;
            }
        }

        public void LoadAllScene()
        {
            AddScene(SceneType.Intro, new IntroScene());
            AddScene(SceneType.MainMenu, new MainScene());
            AddScene(SceneType.Shop, new ShopScene());
            AddScene(SceneType.Inventory, new InventoryScene());
            AddScene(SceneType.Equipment, new EquipmentScene());
            AddScene(SceneType.Status, new StatusScene());
            AddScene(SceneType.Buy, new BuyScene());
            AddScene(SceneType.Sell, new SellScene());
            AddScene(SceneType.Dungeon, new DungeonScene());
            AddScene(SceneType.Rest, new RestScene());
            AddScene(SceneType.DungeonClear, new DungeonClearScene());
            
        }

        public void AddScene(SceneType type, BaseScene scene)
        {
            if (!_sceneDictionary.ContainsKey(type))
            {
                _sceneDictionary.Add(type, scene);
            }
        }
        public void RemoveScene(SceneType type)
        {
            if (_sceneDictionary.ContainsKey(type))
            {
                _sceneDictionary.Remove(type);
            }
        }

        public void ChangeScene(SceneType type)
        {
            if (_sceneDictionary.ContainsKey(type))
            {
                _currentScene?.End();
                _currentScene = _sceneDictionary[type];


                _sceneDictionary[type].Start();
            }
        }

        public BaseScene? GetCurrentScene()
        {
            return _currentScene;
        }
    }
}
