namespace Pathfinder2e.Containers
{

    public class BuildBlock
    {
        public int level { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string data { get; set; }
        public BuildBlock child { get; set; }
    }

}
