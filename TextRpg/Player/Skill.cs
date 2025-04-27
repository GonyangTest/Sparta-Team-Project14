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
        
        public override string ToString()
        {
            return $"이름 : {Name} | 설명 : {Description} | 스킬 계수 : {PowerMultiplier} | 마나 소모 : {ManaCost}";
        }
    }
}