using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
namespace TextRpg
{
    public enum SkillType
    {
        Single,
        AoE
    }

    public class Skill
    {
        public int Id { get; set; }
        public string Job;
        public string Name;
        public string Description { get; set; }
        public float PowerMultiplier;
        public int ManaCost;
        public SkillType Type;

        public Skill(string job, string name, float powerMultiplier, int manaCost, SkillType type)
        {
            Job = job;
            Name = name;
            PowerMultiplier = powerMultiplier;
            ManaCost = manaCost;
            Type = type;
        }

        // TODO: 나중에 삭제 필요
        public static Dictionary<int, Skill> SkillList = new Dictionary<int, Skill>()
        {
            {1, new Skill(GameConstance.Job.WARRIOR, "강타", 2f, 10, SkillType.Single) },
            {2, new Skill(GameConstance.Job.WARRIOR, "대지의분노", 1.5f, 20, SkillType.AoE) },
            {3, new Skill(GameConstance.Job.THIEF, "기습", 2f, 10, SkillType.Single) },
            {4, new Skill(GameConstance.Job.THIEF, "그림자 춤", 1.5f, 20, SkillType.AoE) },
            {5, new Skill(GameConstance.Job.ARCHER, "더블샷", 2f, 10, SkillType.Single) },
            {6, new Skill(GameConstance.Job.ARCHER, "화살비", 1.5f, 20, SkillType.AoE) },
            {7, new Skill(GameConstance.Job.MAGE, "파이어볼", 2f, 10, SkillType.Single) },
            {8, new Skill(GameConstance.Job.MAGE, "메테오", 1.5f, 20, SkillType.AoE) }
        };
        
        public override string ToString()
        {
            return $"이름 : {Name} | 설명 : {Description} | 스킬 계수 : {PowerMultiplier} | 마나 소모 : {ManaCost}";
        }
    }
}