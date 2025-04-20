using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Dungeon;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class DungeonScene : BaseScene
    {
        private const string TITLE = "던전입장";
        private const string DESCRIPTION = "이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.";


        protected override void RegisterMenu()
        {
            AddMenuAction(1, MenuType.easy, () => {
                GameManager.Instance.EnterDungeon(DungeonType.Easy);
                SceneManager.Instance.ChangeScene(SceneType.DungeonClear); 
            });
            AddMenuAction(2, MenuType.normal, () => { 
                GameManager.Instance.EnterDungeon(DungeonType.Normal);
                SceneManager.Instance.ChangeScene(SceneType.DungeonClear);
            });
            AddMenuAction(3, MenuType.hard, () => { 
                GameManager.Instance.EnterDungeon(DungeonType.Hard);
                SceneManager.Instance.ChangeScene(SceneType.DungeonClear);
            });
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.MainMenu); });
        }

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine(DESCRIPTION);
            Console.WriteLine();
            ViewMenu();
        }
    }
}
