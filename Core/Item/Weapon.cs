using System;
using Newtonsoft.Json;

namespace SpartaDungeon.Core.Item
{
    [Serializable]
    public class Weapon : BaseItem
    {
        public Weapon(int id, string name, string description, int price, ItemOptions options) : base(id, name, description, price, options)
        {
            _itemType = ItemType.Weapon;
            _isEquippable = 1;
        }
    }
}
