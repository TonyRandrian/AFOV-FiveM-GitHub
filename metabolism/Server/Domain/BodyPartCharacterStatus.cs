using System;

namespace metabolism.Server.Domain
{
    public class BodyPartCharacterStatus
    {
        public BodyPartDefinition BodyPart { get; set; }
        public float Health { get; set; }


        public BodyPartCharacterStatus(BodyPartDefinition bodyPart)
        {
            BodyPart = bodyPart;
            Health = bodyPart.MaxHealth;
        }


        public void Damage(float damage)
        {
            Health = Math.Max(0, Health - damage);
        }

        public void Heal(float damage)
        {
            Health = Math.Min(100, Health + damage);
        }
    }
}
