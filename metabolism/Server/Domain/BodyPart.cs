using System;

namespace metabolism.Server.Domain
{
    public class BodyPart
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public float MaxHealth { get; set; }
        public float HealthCriticalThreshold { get; set; }
        public float WeaponDamageMultiplier { get; set; }


        public BodyPart() { }
    }
}
