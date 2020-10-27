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

}
