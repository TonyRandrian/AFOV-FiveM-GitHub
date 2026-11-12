using System;

namespace metabolism.Server.Domain
{
    public class BodyPartDefinition
    {
        public Guid Id { get; set; }
        public BodyPartEnum BodyPart { get; set; } 
        public float MaxHealth { get; set; }
        public float HealthCriticalThreshold { get; set; }
        public float WeaponDamageMultiplier { get; set; }


        public BodyPartDefinition() { }
    }
}
