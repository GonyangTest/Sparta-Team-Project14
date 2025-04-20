using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Utils;

namespace SpartaDungeon.Core.UI.Scene
{
    public abstract class BaseScene
    {

        // 메뉴 리스트 및 액션 리스트 저장 변수
        protected Dictionary<int, MenuType> MenuList = new Dictionary<int, MenuType>();
        protected Dictionary<int, Action> MenuActionList = new Dictionary<int, Action>();
        protected HashSet<int> InputKeys = new HashSet<int>();

        protected BaseScene()
        {
            RegisterMenu();
        }

        // 메뉴 등록
        protected virtual void RegisterMenu() { }

        // 메뉴 출력
        protected void ViewMenu()
        {
            List<int> Keys = MenuList.Keys.ToList(); // NOTE: 등록 순서대로 메뉴 표시
            //Keys.Sort();

            foreach (int key in Keys)
            {
                Console.WriteLine($"{key}. {EnumUtils.GetDescription(MenuList[key])}");
            }
        }

        // 메뉴 추가
        protected void AddMenuAction(int index, MenuType menu, Action action)
        {
            MenuList.Add(index, menu);
            MenuActionList.Add(index, action);
            InputKeys.Add(index);
        }

        protected void AddAction(int index, Action action)
        {
            MenuActionList[index] = action;
            InputKeys.Add(index);
        }

        // 메뉴 실행
        public void ExecuteMenuAction(int index)
        {
            MenuActionList[index].Invoke();
        }
        


        // 씬 기본 구성
        protected abstract void DisplayScene();
        public virtual void Start() { }
        public virtual void Update()
        {
            DisplayScene();
        }
        public virtual void HandleInput(string input)
        {
            if (int.TryParse(input, out int index))
            {
                if (InputKeys.Contains(index))
                {
                    ExecuteMenuAction(index);
                }
                else
                {
                    Update();
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
            else
            {
                Update();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
        public virtual void End() { Console.Clear(); }
    }
}
