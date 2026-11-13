using CitizenFX.Core;
using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Mappers;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;

namespace metabolism.Server.Application.Services
{
    public class BodyPartService
    {
        private readonly PlayerRegistry _registry;
        private readonly IClientNotifier _notifier;
        private readonly BoneBodyPartMapper _boneMapper;


        public BodyPartService(
            PlayerRegistry registry,
            IClientNotifier notifier,
            BoneBodyPartMapper boneMapper)
        {
            _registry = registry;
            _notifier = notifier;
            _boneMapper = boneMapper;
        }

        public void ApplyBoneDamage(string handle, int boneId, float damage)
        {
            UpdateBodyPart(handle, _boneMapper.ToBodyPart(boneId), damage);
            Debug.WriteLine($"{_boneMapper.ToBodyPart(boneId)} : {damage}");
        }

        public void UpdateBodyPart(string handle, BodyPartEnum bodyPart, float damage)
        {
            if (_registry.TryGet(handle, out PlayerState state))
            {
                foreach (BodyPartCharacterStatus status in state.BodyPartsStatus)
                {
                    if (status.BodyPart.BodyPart == bodyPart)
                    {
                        status.Damage(damage);
                        _notifier.SendBodyParts(state);

                        return;
                    }
                }
            }
        }
    }
}
