using System;

namespace metabolism.Server.Domain
{
    public class CharacterGauge
    {
        public Guid Id { get; set; }
        public Character Character { get; set; }
        public Gauge Gauge { get; set; }
        public float ActualValue { get; set; }
        public float MaxValue { get; set; }
        public float VariationRegen { get; set; }
        public float VariationDrain { get; set; }


        public CharacterGauge() { }
    }
}
