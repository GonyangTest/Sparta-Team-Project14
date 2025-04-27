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
            public const double INITIAL_EXP = 0;
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
            // 치명타 데미지 배율
            public const float CRITICAL_DAMAGE_MULTIPLIER = 1.6f;
            // 공격 후 대기 시간
            public const int SLEEP_AFTER_ATTACK = 500;
            // 스킬 사용 후 대기 시간
            public const int SLEEP_AFTER_SKILL = 300;
            // 몬스터 사라진 후 대기 시간
            public const int SLEEP_AFTER_DEATH = 2000;
            // 던전 나가기 전 대기 시간
            public const int SLEEP_DUNGEON_EXIT = 1000;
        }

        public static class Monster
        {

            // 회피 확률
            public const int EVASION_CHANCE = 10;
            // 몬스터 최대 수
            public const int MAX_MONSTER_COUNT = 4;
        }
        
        public static class Dungeon
        {
            public const int MAX_STAGE = 10;
            public const string MONSTER_CSV_PATH = @"..\..\..\monsters.csv";
        }

        public static class Rest
        {
            public const int USE_POTION_SLEEP_TIME = 300;
        }

        public static class Sound
        {
            public const string MAIN_SOUND_PATH = "Sounds/Main.mp3";
            public const string DUNGEON_SOUND_PATH = "Sounds/Dungeon.mp3";

            public const float MAIN_SOUND_VOLUME = 0.05f;
            public const float DUNGEON_SOUND_VOLUME = 0.05f;
        }

        public static class Inventory
        {
            public const int ITEM_EQUIP_SLEEP = 800;
        }
    }
}
