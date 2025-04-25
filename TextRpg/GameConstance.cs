using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRpg;

namespace TextRpg
{
    public static class GameConstance
    {
        public static class Player
        {
            public const int INITIAL_GOLD = 10000;
            public const int INITIAL_HEALTH = 100;
            public const int INITIAL_EXP = 0;
            public const int INITIAL_LEVEL = 1;
            public const int INITIAL_MAX_EXP = 10;
            public const float CRITICAL_DAMAGE_MULTIPLIER = 1.6f;
            public const float LEVEL_UP_POWER_INCREASE = 0.5f;
            public const int LEVEL_UP_DEFENSE_INCREASE = 1;
        }

        public static class Job
        {
            public const string WARRIOR = "전사";
            public const string THIEF = "도적";
            public const string ARCHER = "궁수";
            public const string MAGE = "마법사";
        }

        public static class Skill
        {
            public const int WARRIOR_SKILL_1_MANA_COST = 10;
            public const int WARRIOR_SKILL_2_MANA_COST = 20;
            public const int THIEF_SKILL_1_MANA_COST = 10;
            public const int THIEF_SKILL_2_MANA_COST = 20;
            public const int ARCHER_SKILL_1_MANA_COST = 10;
            public const int ARCHER_SKILL_2_MANA_COST = 20;
            public const int MAGE_SKILL_1_MANA_COST = 10;
            public const int MAGE_SKILL_2_MANA_COST = 20;
            
            // 스킬 데미지 배율
            public const float SKILL_SINGLE_POWER_MULTIPLIER = 2.0f;
            public const float SKILL_AOE_POWER_MULTIPLIER = 1.5f;
        }

        public static class Item
        {
            // 소비 아이템
            public const string HEALTH_POTION_NAME = "체력포션";
            public const string MANA_POTION_NAME = "마나포션";
            public const int HEALTH_POTION_PRICE = 50;
            public const int MANA_POTION_PRICE = 100;
            public const int HEALTH_POTION_HEAL_AMOUNT = 30;
            public const int MANA_POTION_HEAL_AMOUNT = 30;
            public const int INITIAL_POTION_COUNT = 3;
            
            // 판매 가격 배율
            public const float SELL_ITEM_PRICE_MULTIPLIER = 0.85f;
        }

        public static class Battle
        {
            // 회피 확률
            public const int EVASION_CHANCE = 10;
            // 치명타 확률
            public const int CRITICAL_CHANCE = 15; // NOTE: 플레이어 치명타 확률로 계산할지?
            // 치명타 데미지 배율
            public const float CRITICAL_DAMAGE_MULTIPLIER = 1.6f;
            // 최소 데미지
            public const int MIN_DAMAGE = 1;
            // 공격 후 대기 시간
            public const int SLEEP_AFTER_ATTACK = 500;
            // 스킬 사용 후 대기 시간
            public const int SLEEP_AFTER_SKILL = 300;
            // 몬스터 사라진 후 대기 시간
            public const int SLEEP_AFTER_DEATH = 2000;
            // 던전 나가기 전 대기 시간
            public const int SLEEP_DUNGEON_EXIT = 1000;
        }
        
        public static class Dungeon
        {
            public const int MAX_STAGE = 10;
            public const string MONSTER_CSV_PATH = @"..\..\..\monsters.csv";
        }

        public static class Shop
        {
            public const int USE_POTION_SLEEP_TIME = 300;
        }
    }
}
