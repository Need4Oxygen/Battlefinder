using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Pathfinder2e.Containers
{

    public class ArmorPiece
    {
        public string name { get; set; }
        public string descr { get; set; }
        public string category { get; set; }
        public int level { get; set; }
        public float price_gp { get; set; }
        public string price_text { get; set; }
        public int ac_bonus { get; set; }
        public int dex_cap { get; set; }
        public int check_penalty { get; set; }
        public int speed_penalty { get; set; }
        public int check_strength { get; set; }
        public string bulk { get; set; }
        public string group { get; set; }
        public List<string> traits { get; set; }
        public List<Source> source { get; set; }
    }

    public class ArmorGroup
    {
        public string name { get; set; }
        public string descr { get; set; }
        public List<Source> source { get; set; }
    }

}
