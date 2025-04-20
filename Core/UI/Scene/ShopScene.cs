using SpartaDungeon.Core.Enums;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class ShopScene : BaseScene
    {
        private const string TITLE = "상점";
        private const string DESCRIPTION = "필요한 아이템을 얻을 수 있는 상점입니다.";
        private const string GOLD = "\n[보유골드]";
        private const string ITEM_LIST = "\n[아이템 목록]";


        protected override void RegisterMenu()
        {
            // 상태보기
            AddMenuAction(1, MenuType.Buy, () => { SceneManager.Instance.ChangeScene(SceneType.Buy); });
            AddMenuAction(2, MenuType.Sell, () => { SceneManager.Instance.ChangeScene(SceneType.Sell); });
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.MainMenu); });
        }

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine(DESCRIPTION);
            Console.WriteLine(GOLD);
            Console.WriteLine(GameManager.Instance.GetPlayerGold() + " G");
            Console.WriteLine(ITEM_LIST);
            Console.WriteLine(GameManager.Instance.GetShopItemsAsString(false));
            Console.WriteLine();
            ViewMenu();
        }
    }

}
