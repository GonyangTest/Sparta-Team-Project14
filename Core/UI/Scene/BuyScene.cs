using SpartaDungeon.Managers;
using System;
using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Item;

namespace SpartaDungeon.Core.UI.Scene
{
    public class BuyScene : BaseScene
    {
        private const string TITLE = "상점 - 아이템 구매";
        private const string DESCRIPTION = "필요한 아이템을 얻을 수 있는 상점입니다.";
        private const string GOLD = "\n[보유골드]";
        private const string ITEM_LIST = "\n[아이템 목록]";


        protected override void RegisterMenu()
        {
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.Shop); });
        }

        public void RegisterAction()
        {
            List<BaseItem> shopItems = GameManager.Instance.GetShopItems();
            for (int i = 0; i < shopItems.Count; i++)
            {
                int index = i;
                AddAction(index + 1, () =>
                {
                    string result = GameManager.Instance.BuyItem(index);
                    Update();
                    Console.WriteLine(result);
                });
            }
        }

        public override void Update()
        {
            RegisterAction();
            DisplayScene();
        }

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine(DESCRIPTION);
            Console.WriteLine(GOLD);
            Console.WriteLine(GameManager.Instance.GetPlayerGold() + " G");
            Console.WriteLine(ITEM_LIST);
            Console.WriteLine(GameManager.Instance.GetShopItemsAsString(true));
            Console.WriteLine();
            ViewMenu();
        }
    }

}
