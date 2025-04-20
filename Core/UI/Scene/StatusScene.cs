using SpartaDungeon.Core.Enums;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class StatusScene : BaseScene
    {
        private const string TITLE = "상태보기";
        private const string DESCRIPTION = "캐릭터의 정보가 표시됩니다.";


        protected override void RegisterMenu()
        {
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.MainMenu); });
        }

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine(DESCRIPTION);
            PrintPlayerStatus();
            ViewMenu();
        }

        private void PrintPlayerStatus()
        {
            Console.WriteLine(GameManager.Instance.GetPlayerStatus());
        }
    }
}
