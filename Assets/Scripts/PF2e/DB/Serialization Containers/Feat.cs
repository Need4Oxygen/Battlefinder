using System;
using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public sealed class Feat
    {
        public string name { get; set; }
        public string descr { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string cost { get; set; }
        public string actioncost { get; set; }
        public string trigger { get; set; }
        public string frequency { get; set; }
        public List<Prerequisite> prerequisites { get; set; }
        public string requirement { get; set; }
        public List<string> traits { get; set; }
        public List<Lecture> lectures { get; set; }
        public List<Source> source { get; set; }
    }

    public sealed class SkillFeats
    {
        public List<Feat> general { get; set; }
        public List<Feat> varying { get; set; }
        public List<Feat> acrobatics { get; set; }
        public List<Feat> arcana { get; set; }
        public List<Feat> athletics { get; set; }
        public List<Feat> crafting { get; set; }
        public List<Feat> deception { get; set; }
        public List<Feat> diplomacy { get; set; }
        public List<Feat> intimidation { get; set; }
        public List<Feat> lore { get; set; }
        public List<Feat> medicine { get; set; }
        public List<Feat> nature { get; set; }
        public List<Feat> occultism { get; set; }
        public List<Feat> performance { get; set; }
        public List<Feat> religion { get; set; }
        public List<Feat> society { get; set; }
        public List<Feat> stealth { get; set; }
        public List<Feat> survival { get; set; }
        public List<Feat> thievery { get; set; }

        public List<Feat> Find(string type)
        {
            switch (type)
            {
                case "general feats": return general;
                case "skill feats":
                    List<Feat> all = new List<Feat>(varying);
                    all.AddRange(acrobatics);
                    all.AddRange(arcana);
                    all.AddRange(athletics);
                    all.AddRange(crafting);
                    all.AddRange(deception);
                    all.AddRange(diplomacy);
                    all.AddRange(intimidation);
                    all.AddRange(lore);
                    all.AddRange(medicine);
                    all.AddRange(nature);
                    all.AddRange(occultism);
                    all.AddRange(performance);
                    all.AddRange(religion);
                    all.AddRange(society);
                    all.AddRange(stealth);
                    all.AddRange(survival);
                    all.AddRange(thievery);
                    return all;
                default: return new List<Feat>();
            }
        }
    }

}
