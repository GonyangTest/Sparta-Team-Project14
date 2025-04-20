using System.Collections;
using Newtonsoft.Json;
using SpartaDungeon.Core.Constants;
using SpartaDungeon.Core.Enums;
using SpartaDungeon.Core.Equipment;
using SpartaDungeon.Core.Equipment.Interface;
using SpartaDungeon.Core.Inventory;
using SpartaDungeon.Core.Inventory.Interface;
using SpartaDungeon.Core.Item;
using SpartaDungeon.Core.Utils;

namespace SpartaDungeon.User
{
    [Serializable]
    public struct PlayerStatus
    {
        [JsonProperty]
        public ClassType ClassType;
        [JsonProperty]
        public int Level;
        [JsonProperty]
        public string Name;
        [JsonProperty]
        public float Attack;
        [JsonProperty]
        public int AttackBonus;
        [JsonProperty]
        public int Defense;
        [JsonProperty]
        public int DefenseBonus;
        [JsonProperty]
        public int MaxHealth;
        [JsonProperty]
        public int Health;
        [JsonProperty]
        public int MaxHealthBonus;
        [JsonProperty]
        public int Gold;
        [JsonProperty]
        public int Exp;
        [JsonProperty]
        public int MaxExp;
    }
    [Serializable]
    public class Player
    {
        [JsonProperty]
        private PlayerStatus _playerStatus;
        [JsonProperty]
        private IPlayerInventory _inventory;
        [JsonProperty]
        private IPlayerEquipmentSlot _equipmentSlot;

        public PlayerStatus PlayerStatus
        {
            get { return _playerStatus; }
            set { _playerStatus = value; }
        }
        public IPlayerInventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public IPlayerEquipmentSlot EquipmentSlot
        {
            get { return _equipmentSlot; }
            set { _equipmentSlot = value; }
        }


        public Player(IPlayerInventory inventory, IPlayerEquipmentSlot equipmentSlot)
        {
            _inventory = inventory;
            _equipmentSlot = equipmentSlot;
        }

        public void Init()
        {
            _playerStatus.ClassType = ClassType.Warrior;
            _playerStatus.Level = Global.PlayerInitial.LEVEL;
            _playerStatus.Name = Global.PlayerInitial.NAME;
            _playerStatus.Attack = Global.PlayerInitial.ATTACK;
            _playerStatus.Defense = Global.PlayerInitial.DEFENSE;
            _playerStatus.MaxHealth = Global.PlayerInitial.MAX_HEALTH;
            _playerStatus.Health = Global.PlayerInitial.MAX_HEALTH;
            _playerStatus.Gold = Global.PlayerInitial.GOLD;
            _playerStatus.Exp = Global.PlayerInitial.EXP;
            _playerStatus.MaxExp = Global.PlayerInitial.MAX_EXP;
        }

        public void EquipItem(int index)
        {
            BaseItem? item = _inventory.RemoveItem(index);
            BaseItem? unrequippedItem = _equipmentSlot.EquipItem(item.ItemType, item);
            if (unrequippedItem != null)
            {
                _inventory.AddItem(unrequippedItem);
            }

            UpdateState();
        }

        public void UnEquipItem(ItemType itemType)
        {
            BaseItem? item = _equipmentSlot.GetItemByIndex(itemType);
            if (item == null) return;

            _equipmentSlot.UnEquipItem(itemType);
            _inventory.AddItem(item);

            UpdateState();
        }

        public void UpdateState() // AddOption으로 변경 필요할듯?
        {
            _playerStatus.AttackBonus = 0;
            _playerStatus.DefenseBonus = 0;
            _playerStatus.MaxHealthBonus = 0;
            List<BaseItem> items = _equipmentSlot.GetItemList();
            foreach (BaseItem item in items) {
                var options = item.Options.Get();
                foreach (var option in options)
                {
                    if (option.Type == ItemOptionType.Power)
                    {
                        _playerStatus.AttackBonus += (int)option.Value;
                    }
                    else if (option.Type == ItemOptionType.Health)
                    {
                        _playerStatus.MaxHealthBonus += (int)option.Value;
                    }
                    else if (option.Type == ItemOptionType.Defense)
                    {
                        _playerStatus.DefenseBonus += (int)option.Value;
                    }
                }
            }

            if(_playerStatus.Health > _playerStatus.MaxHealth + _playerStatus.MaxHealthBonus)
            {
                _playerStatus.Health = _playerStatus.MaxHealth + _playerStatus.MaxHealthBonus;
            }
        }

        public void AddGold(int gold)
        {
            _playerStatus.Gold += gold;
        }


        public bool BuyItem(BaseItem item)
        {
            if (_playerStatus.Gold < item.Price)
            {
                return false;
            }
            _playerStatus.Gold -= item.Price;
            _inventory.AddItem(item);
            return true;
        }
        public void AddHealth(int health)
        {
            _playerStatus.Health += health;

            if (_playerStatus.Health > _playerStatus.MaxHealth + _playerStatus.MaxHealthBonus)
            {
                _playerStatus.Health = _playerStatus.MaxHealth + _playerStatus.MaxHealthBonus;
            }
            else if (_playerStatus.Health < 0)
            {
                _playerStatus.Health = 0;
            }
        }

        public void Recover()
        {
            _playerStatus.Health = _playerStatus.MaxHealth + _playerStatus.MaxHealthBonus;
        }

        public float GetTotalAttack()
        {
            return _playerStatus.Attack + _playerStatus.AttackBonus;
        }
        public int GetTotalDefense()
        {
            return _playerStatus.Defense + _playerStatus.DefenseBonus;
        }
        public int GetCurrentHealth()
        {
            return _playerStatus.Health;
        }

        public int GetTotalHealth()
        {
            return _playerStatus.MaxHealth + _playerStatus.MaxHealthBonus;
        }

        public int GetCurrentGold()
        {
            return _playerStatus.Gold;
        }
        public void AddExp(int exp)
        {
            _playerStatus.Exp += exp;
            
            if (_playerStatus.Exp >= _playerStatus.MaxExp)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            _playerStatus.Level += 1;
            _playerStatus.Attack += Global.LevelUp.ATTACK_INCREASE;
            _playerStatus.Defense += Global.LevelUp.DEFENSE_INCREASE;
            _playerStatus.MaxHealth += Global.LevelUp.MAX_HEALTH_INCREASE;
            _playerStatus.Health = _playerStatus.MaxHealth + _playerStatus.MaxHealthBonus;
            _playerStatus.Exp = 0;
            _playerStatus.MaxExp = _playerStatus.Level + 1;
        }

        public string GetStatusAsString()
        {
            UpdateState();

            return $"\n{_playerStatus.Name} ({_playerStatus.ClassType.GetDescription()} )\n" +
                $"레벨 : {_playerStatus.Level:00}\n" +
                $"공격력 : {GetTotalAttack()} (+{_playerStatus.AttackBonus})\n" +
                $"방어력 : {GetTotalDefense()} (+{_playerStatus.DefenseBonus})\n" +
                $"체 력 : {_playerStatus.Health}/{GetTotalHealth()} (+{_playerStatus.MaxHealthBonus})\n" +
                $"Gold : {_playerStatus.Gold}G\n";
        }
    }
}

