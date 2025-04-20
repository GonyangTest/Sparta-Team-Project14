using SpartaDungeon.Core;
using SpartaDungeon.Core.Dungeon;
using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Utils;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class DungeonClearScene : BaseScene
    {
        private const string SUCCESS_TITLE = "던전 클리어";
        private const string FAIL_TITLE = "던전 클리어";
        private const string DUNGEON_CLEAR_RESULT = "\n[탐험 결과]";


        protected override void RegisterMenu()
        {
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.MainMenu); });
        }

        protected override void DisplayScene()
        {
            DungeonResult? result = GameManager.Instance.DungeonResult;
            if(result == null)
            {
                Console.WriteLine("던전 결과가 없습니다.");
                return;
            }

            Console.Clear();
            string description;

            if (result.IsClear == true)
            {
                description = $"축하합니다!!\n{EnumUtils.GetDescription(result.Level)}을 클리어 하였습니다.";
                Console.WriteLine(SUCCESS_TITLE);
            }
            else
            {
                description = $"안타깝네요!!\n{EnumUtils.GetDescription(result.Level)}을 클리어 하지 못하였습니다.";
                Console.WriteLine(FAIL_TITLE);


            }

            Console.WriteLine(description);
            Console.WriteLine(DUNGEON_CLEAR_RESULT);
            Console.WriteLine($"체력 {result.PreviousHealth} -> {result.PreviousHealth - result.HealthLost}");
            Console.WriteLine($"Gold {result.PreviousGold} G -> {result.PreviousGold + result.Reward} G");
            Console.WriteLine();
            ViewMenu();
        }
    }
}
