using System;
using System.Collections.Generic;

namespace metabolism.Server.Domain
{
    public class Character
    {
        public Guid Id { get; set; }
        public IEnumerable<string> Identifiers { get; set; }
        public string Nickname { get; set; }
        public bool IsOnline { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAd { get; set; }


        public Character() { }
    }
}
