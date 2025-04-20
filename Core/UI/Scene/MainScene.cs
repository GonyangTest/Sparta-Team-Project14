using SpartaDungeon.Core.Enums;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class MainScene : BaseScene
    {
        private const string MAIN = @"
스파르타 마을에 오신 여러분 환영합니다.
이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.
        ";


        protected override void RegisterMenu()
        {
            AddMenuAction(1, MenuType.Status, () => { SceneManager.Instance.ChangeScene(SceneType.Status); });
            AddMenuAction(2, MenuType.Inventory, () => { SceneManager.Instance.ChangeScene(SceneType.Inventory); });
            AddMenuAction(3, MenuType.Shop, () => { SceneManager.Instance.ChangeScene(SceneType.Shop); });
            AddMenuAction(4, MenuType.Dungeon, () => { SceneManager.Instance.ChangeScene(SceneType.Dungeon); });
            AddMenuAction(5, MenuType.Rest, () => { SceneManager.Instance.ChangeScene(SceneType.Rest); });
            AddMenuAction(6, MenuType.SaveGame, () => {
                GameManager.Instance.SaveGame();
                Update();
                Console.WriteLine("게임이 저장되었습니다.");
            });
        }

        public override void Update()
        {
            DisplayScene();
        }

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(MAIN);
            ViewMenu();
        }
    }
}
