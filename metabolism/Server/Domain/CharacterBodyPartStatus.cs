using System;

namespace metabolism.Server.Domain
{
    public class CharacterBodyPartStatus
    {
        public Guid Id { get; set; }
        public Character Character { get; set; }
        public BodyPart BodyPart { get; set; }
        public float Health { get; set; }


        public CharacterBodyPartStatus() { }
    }
}
