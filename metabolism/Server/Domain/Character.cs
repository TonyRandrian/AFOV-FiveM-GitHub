using System;
using System.Collections.Generic;

namespace metabolism.Server.Domain
{
    public class Character
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string License { get; set; }
        public IReadOnlyList<string> Identifiers { get; set; } = new List<string>();
        public string Nickname { get; set; }
        public bool IsOnline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAd { get; set; } = DateTime.UtcNow;


        public Character() { }
    }
}
