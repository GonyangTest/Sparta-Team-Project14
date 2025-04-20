using System.ComponentModel;
using System.Reflection;

namespace SpartaDungeon.Core.Constants
{
    /// <summary>
    /// 게임 전역 상수 관리
    /// </summary>
    public static class Global
    {
        // UI 관련 상수
        public static string INPUT_PROMPT = "\n원하시는 행동을 입력해주세요.\n>>";
        
        // 플레이어 초기 스탯 상수
        public static class PlayerInitial
        {
            public const int LEVEL = 1;
            public const string NAME = "스파르타 전사";
            public const float ATTACK = 10;
            public const int DEFENSE = 5;
            public const int MAX_HEALTH = 100;
            public const int GOLD = 0;
            public const int EXP = 0;
            public const int MAX_EXP = 1;
        }
        
        // 던전 난이도 관련 상수
        public static class Dungeon
        {
            // 던전 추천 방어력
            public const int EASY_RECOMMENDED_DEFENSE = 5;
            public const int NORMAL_RECOMMENDED_DEFENSE = 11;
            public const int HARD_RECOMMENDED_DEFENSE = 17;
            
            // 던전 보상 골드
            public const int EASY_REWARD_GOLD = 1000;
            public const int NORMAL_REWARD_GOLD = 1700;
            public const int HARD_REWARD_GOLD = 2500;
            
            // 던전 보상 경험치
            public const int REWARD_EXP = 1;
            
            // 던전 실패 확률 (%)
            public const int FAILURE_CHANCE = 40;
            
            // 던전 데미지 관련
            public const int MIN_DAMAGE = 20;
            public const int MAX_DAMAGE = 35;
            
            // 던전 실패 시 체력 감소 비율
            public const float HEALTH_LOSS_ON_FAILURE = 0.5f; // 50%
        }
        
        // 레벨업 관련 상수
        public static class LevelUp
        {
            public const float ATTACK_INCREASE = 0.5f;
            public const int DEFENSE_INCREASE = 1;
            public const int MAX_HEALTH_INCREASE = 10;
        }
        
        // 휴식 관련 상수
        public static class Rest
        {
            public const int REST_COST = 500;
        }
    }
} 