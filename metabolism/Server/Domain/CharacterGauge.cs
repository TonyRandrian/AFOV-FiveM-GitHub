using System;

namespace metabolism.Server.Domain
{
    public class CharacterGauge
    {
        public Guid Id { get; set; }
        public GaugeDefinition Definition { get; }
        public float Value { get; private set; }
        public float Max { get; set; }
        public float DrainPerSecond { get; }
        public float RegenPerSecond { get; }


        public CharacterGauge(GaugeDefinition def)
        {
            Definition = def;
            Max = def.DefaultMaxValue;
            Value = def.DefaultMaxValue;
            DrainPerSecond = def.DefaultVariationDrain;
            RegenPerSecond = def.DefaultVariationRegen;
        }

        public void Drain(float seconds)
        {
            Value = Math.Max(Definition.DefaultMinValue, Value - DrainPerSecond * seconds);
        }

        public void Regen(float seconds)
        {
            Value = Math.Min(Max, Value + RegenPerSecond * seconds);
        }
    }
}