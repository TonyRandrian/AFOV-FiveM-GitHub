using metabolism.Server.Application.Services;

namespace metabolism.Server.Domain
{
    public class DamageZoneShare
    {
        public BodyPartEnum BodyPart { get; set; }
        public float Share { get; set; }


        public DamageZoneShare() { }
    }
}
