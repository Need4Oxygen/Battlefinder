using System;
using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public class Rule
    {
        public List<Element> elements { get; set; }
    }

    public class Element
    {
        public string key { get; set; } // Type of element
        public string from { get; set; }
        public string target { get; set; } // What does it affect
        public string type { get; set; } // How does it stack

        public string value { get; set; }
        public List<Bracket> valueList { get; set; }

        public string diceNumber { get; set; }
        public string dieSize { get; set; }
        public string damageType { get; set; }

        public Predicate predicate { get; set; }
    }

    public class Predicate
    {
        public List<string> all { get; set; }
        public List<string> any { get; set; }
        public List<string> not { get; set; }
    }

    public class ValueList
    {
        public List<Bracket> all { get; set; }
    }

    public class Bracket
    {
        public string start { get; set; }
        public string end { get; set; }
        public string value { get; set; }
    }

}
