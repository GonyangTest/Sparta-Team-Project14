using System;
using Newtonsoft.Json;

namespace SpartaDungeon.Core.Item
{
    [Serializable]
    public class Ring : BaseItem
    {   
        public Ring(int id, string name, string description, int price, ItemOptions options) : base(id, name, description, price, options)
        {
            _itemType = ItemType.Ring;
            _isEquippable = 1;
        }
    }
}
