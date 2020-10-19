namespace Pathfinder2e.Containers
{

    public class BuildBlock
    {
        public int level { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public BuildBlock child { get; set; }
    }

}
