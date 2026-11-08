using metabolism.Server.Models.Gauge;

namespace metabolism.Server.Models
{
    public class Metabolism
    {
        public Hunger Hunger { get; set; }
        public Hygiene Hygiene { get; set; }
        public Needs Needs { get; set; }
        public Sleep Sleep { get; set; }
        public Thirst Thirst { get; set; }
    }
}
