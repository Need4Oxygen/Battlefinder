namespace Pathfinder2e
{

    public sealed class AblBoostData
    {
        // name might be:
        //     "ancestry boost"      "ancestry flaw"     "ancestry free"
        //     "background choice"   "background free"   "class"
        //     "lvl1"                "lvl5"              "lvl10"
        //     "lvl15"               "lvl20"

        public string from { get; set; }
        public string abl { get; set; }
        public int value { get; set; }

        public AblBoostData(string from, string abl, int value)
        {
            this.from = from;
            this.abl = abl;
            this.value = value;
        }
    }

}
