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
            AnimationRule best = null;

            foreach (BodyPartCharacterStatus status in state.BodyPartsStatus)
            {
                InjuryState injuryState = status.GetInjuryState();

                AnimationRule match = _rules
                    .FirstOrDefault(r => r.BodyPart == status.BodyPart.BodyPart
                    && r.State == injuryState);

                if (match != null && (best == null || match.Priority > best.Priority))
                {
                    best = match;
                }
            }

            _notifier.SendMovementClipset(state, best?.MovementClipset);
        }
    }
}
