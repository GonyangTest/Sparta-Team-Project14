using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using System.ComponentModel;
using System.Dynamic;

public enum JobType {
    [Description("전사")]
    Worrior,
    [Description("도적")]
    Thief,
    [Description("궁수")]
    Archer,
    [Description("마법사")]
    Mage
}
namespace TextRpg
{
    public class Job
    {
        private string _name; public string Name { get { return _name;}}
        private int _hp; public int Hp { get { return _hp;}}
        private int _mana; public int Mana { get { return _mana;}}
        private float _power; public float Power { get { return _power;}}
        private int _defense; public int Defense { get { return _defense;}}
        private int _agility; public int Agility { get { return _agility;}}
        private int _criticalChance; public int CriticalChance { get { return _criticalChance;}}

        public Job(string name, int hp, int mana, float power, int defense, int agility, int criticalChance)
        {
            _name = name;
            _hp = hp;
            _mana = mana;
            _power = power;
            _defense = defense;
            _agility = agility;
            _criticalChance = criticalChance;
        }

        public static Dictionary<JobType, Job> JobList = new Dictionary<JobType, Job>()
        {
            { 
                JobType.Worrior, 
                new Job(
                    GameConstance.Job.WARRIOR, 
                    150, 80,
                    8f, 6, 5, 5
                )
            },
            { 
                JobType.Thief, 
                new Job(
                    GameConstance.Job.THIEF, 
                    100, 100,
                    4f, 5, 20, 20
                )
            },
            { 
                JobType.Archer, 
                new Job(
                    GameConstance.Job.ARCHER, 
                    100, 100, 
                    6f, 5, 10, 10
                )
            },
            { 
                JobType.Mage, 
                new Job(
                    GameConstance.Job.MAGE, 
                    80, 150,
                    7f, 4, 5, 10
                )
            }
        };

        public static List<JobType> GetJobTypes()
        {
            return JobList.Keys.ToList();
        }
    }
}