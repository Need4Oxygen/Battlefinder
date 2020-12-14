namespace Pathfinder2e.Containers
{

    public sealed class Source
    {
        public string abbr { get; set; }
        public int page_start { get; set; }
        public int page_stop { get; set; }
    }

    public sealed class SourceInfo
    {
        public string abbr { get; set; }
        public string short_name { get; set; }
        public string full_name { get; set; }
        public string descr { get; set; }
        public string publisher_name { get; set; }
        public string isbn { get; set; }
        public string release_date { get; set; }
        public string is_first_party { get; set; }
        public string pzocode { get; set; }
        public string ogl_copyright_block { get; set; }
    }

}