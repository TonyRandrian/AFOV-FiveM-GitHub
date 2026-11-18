using CitizenFX.Core;
using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Mappers;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;
using System.Collections.Generic;
using System.Linq;

namespace metabolism.Server.Application.Services
{
    public class BodyPartService
    {
        private readonly PlayerRegistry _registry;
        private readonly IClientNotifier _notifier;
        private readonly BoneBodyPartMapper _boneMapper;
        private readonly Dictionary<DamageSource, DamageSourceProfile> _profiles;


        public BodyPartService(
            PlayerRegistry registry,
            IClientNotifier notifier,
            BoneBodyPartMapper boneMapper,
            IDamageSourceConfigProvider damageSourceConfigProvider)
        {
            _registry = registry;
            _notifier = notifier;
            _boneMapper = boneMapper;

            _profiles = damageSourceConfigProvider.LoadDamageSourceProfiles()
                .ToDictionary(profile => profile.Source);
        }

        public void ApplyDamage(string handle, int boneId, float damage, DamageSource source)
        {
            Debug.WriteLine($"Handle : {handle}");
            Debug.WriteLine($"SOURCE : {source}");
            Debug.WriteLine($"Damage : {damage}");

            if (damage <= 0 || source == DamageSource.UNKNOWN)
                return;
            
            if (!_registry.TryGet(handle, out PlayerState state))
                return;

            bool hasProfile = _profiles.TryGetValue(source, out DamageSourceProfile profile);
            float sourceMultiplier = hasProfile
                ? profile.DamageMultiplier : 
                1f;

            // global PV damage
            float globalDamage = damage * sourceMultiplier;
            bool justDied = state.Health.Damage(globalDamage);
            Debug.WriteLine($"Actual health : {state.Health.Health}");

            if (justDied)
            {
                Debug.WriteLine($"[Metabolism] Player {state.Player.Name} is dead.");
                _notifier.SendPlayerDeath(state);
            }

            // just substract the global PV if no profile
            if (!hasProfile)
                return;

            // members PV damage
            float memberDamageMultiplier = 2.0f;
            float memberDamage = globalDamage * memberDamageMultiplier;
            Debug.WriteLine($"Global : {globalDamage}" );
            Debug.WriteLine($"Member : {memberDamage}");

            if (profile.UseBone)
            {
                BodyPartEnum zone = _boneMapper.ToBodyPart(boneId);
                ApplyToZone(state, zone, memberDamage);
            }
            else
            {
                foreach (DamageZoneShare zoneShare in profile.Zones)
                {
                    ApplyToZone(state, zoneShare.BodyPart, memberDamage * zoneShare.Share);
                }
            }

            //_notifier.SendBodyParts(state);
        }

        private static void ApplyToZone(PlayerState state, BodyPartEnum zone, float damage)
        {
            Debug.WriteLine($"zone : {zone}, damage : {damage}");
            BodyPartCharacterStatus status = state.BodyPartsStatus
                .FirstOrDefault(s => s.BodyPart.BodyPart == zone);

            status?.Damage(damage * status.BodyPart.WeaponDamageMultiplier);
        }

        public void RevivePlayer(string handle)
        {
            if (!_registry.TryGet(handle, out PlayerState state))
                return;

            state.Health.Revive();
            Debug.WriteLine($"[Metabolism] Player {state.Player.Name} revived.");
        }
    }
}
