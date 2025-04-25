using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    [Serializable]
    internal class PlayerData
    {
        public string PlayerName { get; set; }
        public string PlayerClass { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int MaxExp { get; set; }
        public int Gold { get; set; }
        public int Hp { get; set; }
        public int Mana { get; set; }
        public int MaxHp { get; set; }
        public int MaxMp { get; set; }
        public float Power { get; set; }
        public int Defense { get; set; }
        public int Agility { get; set; }
        public int CriticalChance { get; set; }

        // 포션 정보
        public int HealthPotionCount { get; set; }
        public int ManaPotionCount { get; set; }

        // 장착 아이템 정보 - 인덱스로 저장
        public int EquippedWeaponIndex { get; set; } = -1;
        public int EquippedArmorIndex { get; set; } = -1;

        public PlayerData() { }

        // Player 객체에서 데이터 추출
        public void FromPlayer(Player player)
        {
            PlayerName = player.playerName;
            PlayerClass = player.playerClass;
            Level = player.level;
            Exp = player.exp;
            MaxExp = player.maxExp;
            Gold = player.gold;
            Hp = player.hp;
            Mana = player.mana;
            MaxHp = player.maxHp;
            MaxMp = player.maxMp;
            Power = player.power;
            Defense = player.defense;
            Agility = player.agility;
            CriticalChance = player.criticalChance;

            // 포션 정보 저장
            HealthPotionCount = player.HealthPotion.Quantity;
            ManaPotionCount = player.ManaPotion.Quantity;
        }

        // PlayerData 객체를 Player 객체에 적용
        public void ToPlayer(Player player)
        {
            player.playerName = PlayerName;
            player.playerClass = PlayerClass;
            player.level = Level;
            player.exp = Exp;
            player.maxExp = MaxExp;
            player.gold = Gold;
            player.hp = Hp;
            player.mana = Mana;
            player.maxHp = MaxHp;
            player.maxMp = MaxMp;
            player.power = Power;
            player.defense = Defense;
            player.agility = Agility;
            player.criticalChance = CriticalChance;

            // 포션 정보 복원
            player.HealthPotion.Quantity = HealthPotionCount;
            player.ManaPotion.Quantity = ManaPotionCount;

            // 직업 정보 설정
            foreach (var job in Job.JobList)
            {
                if (job.Value.Name == PlayerClass)
                {
                    player.SelectedJob = job.Value;
                    break;
                }
            }
        }
    }
}
