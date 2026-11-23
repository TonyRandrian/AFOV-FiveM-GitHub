using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;
using metabolism.Shared;
using System.Collections.Generic;
using System.Linq;

namespace metabolism.Server.Application.Services
{
    public class AnimationService
    {
        private readonly IClientNotifier _notifier;
        private readonly List<AnimationRule> _rules;


        public AnimationService(IClientNotifier notifier, IAnimationConfigProvider configProvider)
        {
            _notifier = notifier;
            _rules = configProvider.LoadAnimationRules().ToList();
        }

        public void Refresh(PlayerState state)
        {
            AnimationRule[] activeRules = state.BodyPartsStatus
                .Select(status =>
                {
                    InjuryState injuryState = status.GetInjuryState();

                    return _rules.FirstOrDefault(r =>
                        r.BodyPart == status.BodyPart.BodyPart &&
                        r.State == injuryState);
                })
                .Where(rule => rule != null)
                .ToArray();

            AnimationRule bestMovement = activeRules
                .Where(r => !string.IsNullOrWhiteSpace(r.MovementClipset))
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault();

            _notifier.SendMovementClipset(
                state,
                bestMovement?.MovementClipset
            );

            AnimationRule bestAnimation = activeRules
                .Where(r => !string.IsNullOrWhiteSpace(r.Dictionary))
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault();

            _notifier.SendAnimation(state, bestAnimation);
        }
    }
}
