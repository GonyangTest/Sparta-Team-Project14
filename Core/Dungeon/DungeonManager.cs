using SpartaDungeon.User;
using System.Text.Json.Serialization;
using System.ComponentModel;
using SpartaDungeon.Core.Dungeon.Interface;
using SpartaDungeon.Core.Constants;

namespace SpartaDungeon.Core.Dungeon
{
    public class DungeonManager : IDungeonManager
    {
        private Player _player;
        private DungeonResult? _dungeonResult;

        public DungeonManager(Player player)
        {
            _player = player;
        }

        /// <summary>
        /// 던전 입장
        /// </summary>
        /// <param name="level">던전 난이도</param>
        public void EnterDungeon(DungeonType level)
        {
            // 던전 난이도에 따른 추천 방어력과 보상 설정
            int recommendedDefense = 0;
            int rewardGold = 0;
            int rewardExp = Global.Dungeon.REWARD_EXP;

            switch (level)
            {
                case DungeonType.Easy:
                    recommendedDefense = Global.Dungeon.EASY_RECOMMENDED_DEFENSE;
                    rewardGold = Global.Dungeon.EASY_REWARD_GOLD;
                    break;
                case DungeonType.Normal:
                    recommendedDefense = Global.Dungeon.NORMAL_RECOMMENDED_DEFENSE;
                    rewardGold = Global.Dungeon.NORMAL_REWARD_GOLD;
                    break;
                case DungeonType.Hard:
                    recommendedDefense = Global.Dungeon.HARD_RECOMMENDED_DEFENSE;
                    rewardGold = Global.Dungeon.HARD_REWARD_GOLD;
                    break;
            }

            int playerDefense = _player.GetTotalDefense();
            bool isSuccess = true;
            int healthLost = 0;

            // 플레이어 현재 상태 저장
            int previousHealth = _player.GetCurrentHealth();
            int previousGold = _player.GetCurrentGold();

            // 방어력이 추천 방어력보다 낮으면 40% 실패 확률
            if (playerDefense < recommendedDefense)
            {
                if (Random.Shared.Next(0, 100) < Global.Dungeon.FAILURE_CHANCE)
                {
                    isSuccess = false;
                }
            }
            // 던전 성공했을때 HP 20~35 피해
            // 방어력이 추천 방어력보다 높을수록 피해가 줄어든다. - 추천 8 방어력 17 -> 20-9~35-9
            if(isSuccess)
            {
                int healthDamage = Random.Shared.Next(Global.Dungeon.MIN_DAMAGE, Global.Dungeon.MAX_DAMAGE + 1);
                healthLost = healthDamage - (playerDefense - recommendedDefense);
                rewardGold = (int)(rewardGold * (1 + _player.GetTotalAttack() / 100));

                // 성공 시 보상 지급
                _player.AddGold(rewardGold);
                _player.AddExp(rewardExp);
                _player.AddHealth(-healthLost);
            }
            // 던전 실패했을때 HP 절반 감소
            else
            {
                int health = _player.GetTotalHealth();
                healthLost = (int)(health * Global.Dungeon.HEALTH_LOSS_ON_FAILURE);
                _player.AddHealth(-healthLost);
            }
            
            // 던전 결과 저장
            _dungeonResult = new DungeonResult
            {
                Level = level,
                Reward = isSuccess ? rewardGold : 0,
                IsClear = isSuccess,
                PreviousGold = previousGold,
                PreviousHealth = previousHealth,
                HealthLost = healthLost
            };
        }

        /// <summary>
        /// 던전 결과 반환
        /// </summary>
        /// <returns>던전 결과</returns>
        public DungeonResult? GetDungeonResult()
        {
            return _dungeonResult;
        }
    }
} 