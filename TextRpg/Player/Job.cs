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
    public class Job
    {
        public string Name;
        public int Hp;
        public int Mana;
        public int MaxHp;
        public int MaxMp;
        public float Power;
        public int Defense;
        public int Agility;
        public int CriticalChance;

        public Job(string name, int hp, int mana, int maxHp, int maxMp, float power, int defense, int agility, int criticalChance)
        {
            Name = name;
            Hp = hp;
            Mana = mana;
            MaxHp = maxHp;
            MaxMp = maxMp;
            Power = power;
            Defense = defense;
            Agility = agility;
            CriticalChance = criticalChance;
        }

        public static Dictionary<int, Job> JobList = new Dictionary<int, Job>()
        {
            {1, new Job(GameConstance.Job.WARRIOR, 150, 80, 150, 80, 8f, 6, 5, 5)},
            {2, new Job(GameConstance.Job.THIEF, 100, 100, 100, 100, 4f, 5, 20, 20)},
            {3, new Job(GameConstance.Job.ARCHER, 100, 100, 100, 100, 6f, 5, 10, 10)},
            {4, new Job(GameConstance.Job.MAGE, 80, 150, 80, 150, 7f, 4, 5, 10)}
        };
    }
}