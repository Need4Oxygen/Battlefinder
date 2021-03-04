using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public sealed class Class
    {
        public string name { get; set; }
        public string descr { get; set; }
        public int hp { get; set; }
        public List<string> skill_train_strings { get; set; }
        public List<string> key_ability_choices { get; set; }
        public List<RuleElement> elements { get; set; }
        public List<Source> source { get; set; }
    }

    public sealed class ClassFeats
    {
        public List<Feat> alchemist { get; set; }
        public List<Feat> barbarian { get; set; }
        public List<Feat> bard { get; set; }
        public List<Feat> champion { get; set; }
        public List<Feat> cleric { get; set; }
        public List<Feat> druid { get; set; }
        public List<Feat> fighter { get; set; }
        public List<Feat> monk { get; set; }
        public List<Feat> ranger { get; set; }
        public List<Feat> rogue { get; set; }
        public List<Feat> sorcerer { get; set; }
        public List<Feat> wizard { get; set; }

        public List<Feat> Find(string className)
        {
            switch (className)
            {
                case "Alchemist": return alchemist;
                case "Barbarian": return barbarian;
                case "Bard": return bard;
                case "Champion": return champion;
                case "Cleric": return cleric;
                case "Druid": return druid;
                case "Fighter": return fighter;
                case "Monk": return monk;
                case "Ranger": return ranger;
                case "Rogue": return rogue;
                case "Sorcerer": return sorcerer;
                case "Wizard": return wizard;
                default: return new List<Feat>();
            }
        }
    }

    public sealed class ClassProgression
    {
        public string name { get; set; }
        public List<ClassStage> progression { get; set; }
    }

    public sealed class ClassStage
    {
        public int level { get; set; }
        public List<string> items { get; set; }
    }

}