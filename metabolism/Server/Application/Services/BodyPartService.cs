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

            if (damage <= 0)
            {
                Debug.WriteLine("damage <= 0");
                return;
            }

            if (!_registry.TryGet(handle, out PlayerState state))
            {
                Debug.WriteLine("!_registry.TryGet(handle, out PlayerState state");
                return;
            }

            if (!_profiles.TryGetValue(source, out DamageSourceProfile profile))
            {
                Debug.WriteLine("Others");
                // not handled source
                return;
            }

            if (profile.UseBone)
            {
                BodyPartEnum zone = _boneMapper.ToBodyPart(boneId);
                ApplyToZone(state, zone, damage * profile.DamageMultiplier);
            }
            else
            {
                foreach (DamageZoneShare zoneShare in profile.Zones)
                {
                    ApplyToZone(state, zoneShare.BodyPart, damage * profile.DamageMultiplier * zoneShare.Share);
                }
            }
        }

        private static void ApplyToZone(PlayerState state, BodyPartEnum zone, float damage)
        {
            Debug.WriteLine($"zone : {zone}, damage : {damage}");
            BodyPartCharacterStatus status = state.BodyPartsStatus
                .FirstOrDefault(s => s.BodyPart.BodyPart == zone);

            status?.Damage(damage * status.BodyPart.WeaponDamageMultiplier);
        }
    }
}
