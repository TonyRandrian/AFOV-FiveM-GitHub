using System;

namespace metabolism.Server.Domain
{
    public class GaugeDefinition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? UpperCriticalThreshold { get; set; }
        public float LowerCriticalThreshold { get; set; }
        public float DefaultMaxValue { get; set; }
        public float DefaultMinValue { get; set; }
        public float DefaultVariationRegen { get; set; }
        public float DefaultVariationDrain { get; set; }
        public GaugeVariationCondition VariationCondition { get; set; }
        public GaugeDisplayType DisplayType { get; set; }
    }
}
