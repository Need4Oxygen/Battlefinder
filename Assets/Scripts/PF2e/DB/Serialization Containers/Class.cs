using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public sealed class Class
    {
        public string name { get; set; }
        public string descr { get; set; }
        public int hp { get; set; }
        public List<string> key_ability_choices { get; set; }
        public List<Lecture> perception { get; set; }
        public List<Lecture> saves { get; set; }
        public List<Lecture> attacks { get; set; }
        public List<Lecture> defenses { get; set; }
        public List<Lecture> skills { get; set; }
        public List<Lecture> class_dc_and_spells { get; set; }
        public List<Source> source { get; set; }
    }

    public sealed class ClassFeatures
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