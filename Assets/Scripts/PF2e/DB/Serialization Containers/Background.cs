using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public sealed class Background
    {
        public string name { get; set; }
        public string descr { get; set; }
        public List<string> ability_boost_choices { get; set; }
        public List<Lecture> lectures { get; set; }
        public List<string> free_skill_feats { get; set; }
        public bool community_licenced { get; set; }
        public bool is_specific_to_adv { get; set; }
        public List<Source> source { get; set; }
    }

}