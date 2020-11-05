namespace Pathfinder2e.Containers
{

    public class Trait
    {
        public string name { get; set; }
        public string descr { get; set; }
        public string type { get; set; }
        public Source source { get; set; }

        public Trait(string name, string descr, string type, Source source)
        {
            this.name = name;
            this.descr = descr;
            this.type = type;
            this.source = source;
        }
    }

    public class TraitFull : Trait
    {
        public string from { get; set; }

        public TraitFull(Trait trait, string from) : base(null, null, null, null)
        {
            this.name = trait.name;
            this.descr = trait.descr;
            this.type = trait.type;
            this.source = trait.source;
            this.from = from;
        }

        public TraitFull(string name, string descr, string type, Source source, string from) : base(name, descr, type, source)
        {
            this.name = name;
            this.descr = descr;
            this.type = type;
            this.source = source;
            this.from = from;
        }

    }

}
