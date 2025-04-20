using SpartaDungeon.Managers;
using SpartaDungeon.Core.Enums;

namespace SpartaDungeon.Core.UI.Scene
{
    public class InventoryScene : BaseScene
    {
        private const string TITLE = "인벤토리";
        private const string DESCRIPTION = "보유 중인 아이템을 관리할 수 있습니다.";
        private const string ITEM_LIST = "\n[아이템 목록]";

        protected override void RegisterMenu()
        {
            AddMenuAction(1, MenuType.Equipment, () => { SceneManager.Instance.ChangeScene(SceneType.Equipment); });
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.MainMenu); });
        }

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine(DESCRIPTION);
            Console.WriteLine(ITEM_LIST);
            Console.WriteLine(GameManager.Instance.GetEquipmentAsString(false));
            Console.WriteLine(GameManager.Instance.GetInventoryAsString(false));
            Console.WriteLine();
            ViewMenu();
        }
    }

}
