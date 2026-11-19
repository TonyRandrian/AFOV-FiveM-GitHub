using System;
using System.Collections.Generic;

namespace metabolism.Server.Domain
{
    public class BodyPartDefinition
    {
        public Guid Id { get; set; }
        public BodyPartEnum BodyPart { get; set; } 
        public float MaxHealth { get; set; }
        public List<InjuryThreshold> InjuryThresholds { get; set; } = new List<InjuryThreshold>();
        public float WeaponDamageMultiplier { get; set; }


        public BodyPartDefinition() { }
    }
}
