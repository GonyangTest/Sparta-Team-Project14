using SpartaDungeon.Core.Enums;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class IntroScene : BaseScene
    {
        private const string TITLE = @"
            ███████╗██████╗  █████╗ ██████╗ ████████╗ █████╗         ██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ███████╗ ██████╗ ███╗   ██╗
            ██╔════╝██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝██╔══██╗        ██╔══██╗██║   ██║████╗  ██║██╔════╝ ██╔════╝██╔═══██╗████╗  ██║
            ███████╗██████╔╝███████║██████╔╝   ██║   ███████║        ██║  ██║██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██║   ██║██╔██╗ ██║
            ╚════██║██╔═══╝ ██╔══██║██╔══██╗   ██║   ██╔══██║        ██║  ██║██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║   ██║██║╚██╗██║
            ███████║██║     ██║  ██║██║  ██║   ██║   ██║  ██║        ██████╔╝╚██████╔╝██║ ╚████║╚██████╔╝███████╗╚██████╔╝██║ ╚████║
            ╚══════╝╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝        ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝
    
                             
                                       ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                       ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣒⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                       ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣪⠀⡀⣀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⣀⢀⢀⢀⡀⡀⡀⠀⠀⠀
                                       ⠐⠲⠇⠷⠸⠽⠨⠷⠪⠗⣅⠖⡿⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⣭⢽⠽⣟⠯⠆⠀
                                       ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢵⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠀⠀⠀
                                       ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠣⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
        ";

        protected override void DisplayScene()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            ViewMenu();
        }

        protected override void RegisterMenu()
        {
            AddMenuAction(1, MenuType.NewGame, () => {
                GameManager.Instance.NewGame();
                SceneManager.Instance.ChangeScene(SceneType.MainMenu); 
            });
            AddMenuAction(2, MenuType.LoadGame, () => {
                if(GameManager.Instance.LoadGame())
                {
                    SceneManager.Instance.ChangeScene(SceneType.MainMenu); 
                }
            });
            AddMenuAction(3, MenuType.GameExit, () => { Environment.Exit(0); });
        }
    }
}
