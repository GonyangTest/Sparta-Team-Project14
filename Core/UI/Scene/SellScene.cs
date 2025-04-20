using SpartaDungeon.Managers;
using System;
using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Item;

namespace SpartaDungeon.Core.UI.Scene
{
    public class SellScene : BaseScene
    {
        private const string TITLE = "상점 - 아이템 판매";
        private const string DESCRIPTION = "필요한 아이템을 얻을 수 있는 상점입니다.";
        private const string GOLD = "\n[보유골드]";
        private const string ITEM_LIST = "\n[아이템 목록]";
        private List<BaseItem>? equipmentItems;
        private List<BaseItem>? inventoryItems;


        protected override void RegisterMenu()
        {
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.Shop); });
        }

        public void RegisterAction()
        {
            int equipmentItemsCount = 0;
            if (equipmentItems != null)
            {
                equipmentItemsCount = equipmentItems.Count;
                for (int i = 0; i < equipmentItemsCount; i++)
                {
                    int index = i;
                    AddAction(index + 1, () =>
                    {
                        string result = GameManager.Instance.SellItem(index, equipmentItems[index], true);
                        Update();
                        Console.WriteLine(result);
                    });
                }
            }
            if (inventoryItems != null)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    int index = i;
                    AddAction(index + equipmentItemsCount + 1, () =>
                    {
                        string result = GameManager.Instance.SellItem(index, inventoryItems[index], false);
                        Update();
                        Console.WriteLine(result);
                    });
                }
            }
        }

        public override void Update()
        {
            equipmentItems = GameManager.Instance.GetEquipmentItmes();
            inventoryItems = GameManager.Instance.GetInventoryItmes();

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
            Console.WriteLine(GameManager.Instance.GetEquipmentAsString(true));
            Console.WriteLine(GameManager.Instance.GetInventoryAsString(true));
            Console.WriteLine();
            ViewMenu();
        }
    }

}
