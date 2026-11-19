using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;
using metabolism.Shared;
using System.Collections.Generic;
using System.Linq;

namespace metabolism.Server.Application.Services
{
    public class RestrictionService
    {
        private readonly IClientNotifier _notifier;
        private readonly Dictionary<(BodyPartEnum, InjuryState), ActionRestriction> _rules;


        public RestrictionService(IClientNotifier notifier, IRestrictionConfigProvider configProvider)
        {
            _notifier = notifier;
            _rules = configProvider.LoadRestrictionRules()
                .ToDictionary(rule => (rule.BodyPart, rule.State), rule => rule.Restrictions);
        }

        public void Refresh(PlayerState state)
        {
            ActionRestriction result = ActionRestriction.NONE;

            foreach (BodyPartCharacterStatus status in state.BodyPartsStatus)
            {
                InjuryState injuryState = status.GetInjuryState();

                if (_rules.TryGetValue((status.BodyPart.BodyPart, injuryState), out ActionRestriction restriction))
                {
                    result |= restriction;
                }
            }

            _notifier.SendRestrictions(state, result);
        }
    }
}
