using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Monster
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }

        public override string ToString()
        {
            return $"{Name} (HP: {CurrentHP}/{MaxHP}, ATK: {Attack}, DEF: {Defense}, SPD: {Speed})";
        }
    }
}
