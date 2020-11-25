using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public sealed class Ancestry
    {
        public string name { get; set; }
        public string descr { get; set; }
        public int hp { get; set; }
        public int speed { get; set; }
        public string size { get; set; }
        public List<string> abl_boosts { get; set; }
        public List<string> abl_flaws { get; set; }
        public List<string> languages { get; set; }
        public List<string> traits { get; set; }
        public List<string> ancestry_features { get; set; }
        public List<Source> source { get; set; }
    }

    public sealed class AncestryHeritages
    {
        public List<Feat> dwarf { get; set; }
        public List<Feat> elf { get; set; }
        public List<Feat> gnome { get; set; }
        public List<Feat> goblin { get; set; }
        public List<Feat> halfling { get; set; }
        public List<Feat> human { get; set; }

        public List<Feat> Find(string ancestryName)
        {
            switch (ancestryName)
            {
                case "Dwarf": return dwarf;
                case "Elf": return elf;
                case "Gnome": return gnome;
                case "Goblin": return goblin;
                case "Halfling": return halfling;
                case "Human": return human;
                default: return new List<Feat>();
            }
        }
    }

    public sealed class AncestryFeats
    {
        public List<Feat> dwarf { get; set; }
        public List<Feat> elf { get; set; }
        public List<Feat> gnome { get; set; }
        public List<Feat> goblin { get; set; }
        public List<Feat> halfling { get; set; }
        public List<Feat> human { get; set; }
        public List<Feat> half_elf { get; set; }
        public List<Feat> half_orc { get; set; }

        public List<Feat> Find(string ancestryName)
        {
            switch (ancestryName)
            {
                case "Dwarf": return dwarf;
                case "Elf": return elf;
                case "Gnome": return gnome;
                case "Goblin": return goblin;
                case "Halfling": return halfling;
                case "Human": return human;
                case "Half_elf": return human;
                case "Half_orc": return human;
                default: return new List<Feat>();
            }
        }
    }

}
