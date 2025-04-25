using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public static class SkillFactory
    {
        // 스킬 데이터베이스
        private static Dictionary<int, Skill> _skillDatabase = new Dictionary<int, Skill>();
        
        // 초기화 메서드
        static SkillFactory()
        {
            InitializeSkills();
        }
        
        // 스킬 초기화
        private static void InitializeSkills()
        {
            // 전사 스킬
            RegisterSkill(1, GameConstance.Job.WARRIOR, "강타", "강력한 한 방의 공격으로 단일 대상에게 큰 피해를 입힙니다.", 
                          GameConstance.Skill.SKILL_SINGLE_POWER_MULTIPLIER, 10, SkillType.Single);
            RegisterSkill(2, GameConstance.Job.WARRIOR, "대지의분노", "주변 모든 적에게 대지의 분노를 발산합니다.", 
                          GameConstance.Skill.SKILL_AOE_POWER_MULTIPLIER, 20, SkillType.AoE);
            
            // 도적 스킬
            RegisterSkill(3, GameConstance.Job.THIEF, "기습", "그림자에서 나와 단일 대상에게 불의의 일격으로 공격합니다.", 
                          GameConstance.Skill.SKILL_SINGLE_POWER_MULTIPLIER, 10, SkillType.Single);
            RegisterSkill(4, GameConstance.Job.THIEF, "그림자 춤", "빠른 움직임으로 모든 적에게 연속 공격을 가합니다.", 
                          GameConstance.Skill.SKILL_AOE_POWER_MULTIPLIER, 20, SkillType.AoE);
            
            // 궁수 스킬
            RegisterSkill(5, GameConstance.Job.ARCHER, "더블샷", "두 발의 화살을 연속으로 쏘아 단일 대상에게 피해를 입힙니다.", 
                          GameConstance.Skill.SKILL_SINGLE_POWER_MULTIPLIER, 10, SkillType.Single);
            RegisterSkill(6, GameConstance.Job.ARCHER, "화살비", "여러 발의 화살을 동시에 발사하여 모든 적을 공격합니다.", 
                          GameConstance.Skill.SKILL_AOE_POWER_MULTIPLIER, 20, SkillType.AoE);
            
            // 마법사 스킬
            RegisterSkill(7, GameConstance.Job.MAGE, "파이어볼", "불의 힘을 모아 강력한 화염구를 발사하여 단일 대상에게 피해를 입힙니다.", 
                          GameConstance.Skill.SKILL_SINGLE_POWER_MULTIPLIER, 10, SkillType.Single);
            RegisterSkill(8, GameConstance.Job.MAGE, "메테오", "하늘에서 불덩어리를 떨어뜨려 모든 적을 공격합니다.", 
                          GameConstance.Skill.SKILL_AOE_POWER_MULTIPLIER, 20, SkillType.AoE);
        }
        
        // 스킬 등록 메서드
        private static void RegisterSkill(int id, string job, string name, string description, float powerMultiplier, int manaCost, SkillType type)
        {
            var skill = new Skill(job, name, powerMultiplier, manaCost, type)
            {
                Id = id,
                Description = description
            };
            _skillDatabase[id] = skill;
        }
        
        // ID로 스킬 가져오기
        public static Skill GetSkill(int id)
        {
            if (_skillDatabase.TryGetValue(id, out Skill skill))
            {
                return skill;
            }
            return null;
        }
        
        // 직업별 스킬 가져오기
        public static List<Skill> GetSkillsByJob(string jobName)
        {
            return _skillDatabase.Values
                .Where(skill => skill.Job == jobName)
                .ToList();
        }
    }
} 