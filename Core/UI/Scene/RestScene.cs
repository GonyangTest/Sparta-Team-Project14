using SpartaDungeon.Core.Enums;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class RestScene : BaseScene
    {
        private const string TITLE = "휴식하기";
        private const string DESCRIPTION = "500 G 를 내면 체력을 회복할 수 있습니다.";


        protected override void RegisterMenu()
        {

            AddMenuAction(1, MenuType.Rest, () => {
                string result = GameManager.Instance.Rest();
                Update();
                Console.WriteLine(result);
            });
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.MainMenu); });
        }

        public override void Update()
        {
            DisplayScene();
        }

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.Write(DESCRIPTION);
            Console.WriteLine($" (보유 골드 : {GameManager.Instance.GetPlayerGold()} G)\n");
            ViewMenu();
        }

        private void PrintPlayerStatus()
        {
            Console.WriteLine(GameManager.Instance.GetPlayerStatus());
        }
    }
}
