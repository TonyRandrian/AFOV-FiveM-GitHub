using metabolism.Shared;

namespace metabolism.Server.Domain
{
    public class InjuryThreshold
    {
        public InjuryState State { get; set; }
        public float MinHealth { get; set; }
        public float MaxHealth { get; set; }
    }
}
