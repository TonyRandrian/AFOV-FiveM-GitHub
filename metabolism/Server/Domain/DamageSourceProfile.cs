using System.Collections.Generic;

namespace metabolism.Server.Domain
{
    public class DamageSourceProfile
    {
        public DamageSource Source { get; set; }
        public bool UseBone { get; set; }
        public float DamageMultiplier { get; set; } = 1.0f;
        public List<DamageZoneShare> Zones { get; set; } = new List<DamageZoneShare>();


        public DamageSourceProfile() { }
    }
}
