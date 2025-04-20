using System;
using System.Reflection;
using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Item;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Core.UI.Scene
{
    public class EquipmentScene : BaseScene
    {
        private const string TITLE = "인벤토리 - 장착관리";
        private const string DESCRIPTION = "보유 중인 아이템을 관리할 수 있습니다.";
        private const string ITEM_LIST = "\n[아이템 목록]";
        private List<BaseItem>? equipmentItems;
        private List<BaseItem>? inventoryItems;

        protected override void RegisterMenu()
        {
            AddMenuAction(0, MenuType.Exit, () => { SceneManager.Instance.ChangeScene(SceneType.Inventory); });
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
                        GameManager.Instance.UnEquipItem(equipmentItems[index].ItemType);
                        Update();
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
                        GameManager.Instance.EquipItem(index);
                        Update();
                    });
                }
            }
        }

        public override void Update() {
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
            Console.WriteLine(ITEM_LIST);
            Console.WriteLine(GameManager.Instance.GetEquipmentAsString(true));
            Console.WriteLine(GameManager.Instance.GetInventoryAsString(true));
            Console.WriteLine();
            ViewMenu();
        }
    }

}
