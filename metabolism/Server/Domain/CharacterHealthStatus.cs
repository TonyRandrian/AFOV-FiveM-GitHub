using System;

namespace metabolism.Server.Domain
{
    // player global PV
    public class CharacterHealthStatus
    {
        public float MaxHealth { get; }
        public float Health { get; private set; }
        public bool IsDead => Health <= 0;


        public CharacterHealthStatus(float maxHealth)
        {
            MaxHealth = Math.Max(0, maxHealth);
            Health = MaxHealth;
        }

        public bool Damage(float damage)
        {
            if (damage <= 0 || IsDead)
                return false;

            Health = Math.Max(0, Health - damage);

            return IsDead;
        }

        public void Heal(float amount)
        {
            if (amount <= 0)
                return;

            Health = Math.Min(MaxHealth, Health + amount);
        }

        public void Revive()
        {
            Health = MaxHealth;
        }
    }
}
