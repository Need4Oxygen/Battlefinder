namespace Pathfinder2e.Containers
{

    public class Lecture
    {
        public string target { get; set; }
        public string prof { get; set; }

        public Lecture() { }

        public Lecture(string target, string prof)
        {
            this.target = target;
            this.prof = prof;
        }

    }

    public class LectureFull : Lecture
    {
        public string from { get; set; }

        public LectureFull(Lecture lecture, string from) : base(null, null)
        {
            this.target = lecture.target;
            this.prof = lecture.prof;
            this.from = from;
        }

        public LectureFull(string target, string prof, string from) : base(target, prof)
        {
            this.target = target;
            this.prof = prof;
            this.from = from;
        }
    }

}
