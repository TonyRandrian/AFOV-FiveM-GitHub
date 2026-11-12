using System;

namespace metabolism.Server.Domain
{
    public class Temperature
    {
        public Guid Id { get; set; }
        public int RageId { get; set; }
        public int LowerHourLimit { get; set; }
        public int UpperHourLimit { get; set; }
        public WeatherEnum Weather { get; set; }
        public float Value { get; set; }
    }
}
