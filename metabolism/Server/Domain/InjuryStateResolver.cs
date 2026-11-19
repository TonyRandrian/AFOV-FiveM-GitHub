using metabolism.Shared;
using System.Collections.Generic;
using System.Linq;

namespace metabolism.Server.Domain
{
    public class InjuryStateResolver
    {
        public static InjuryState Resolve(float health, IEnumerable<InjuryThreshold> thresholds)
        {
            InjuryThreshold match = thresholds
                .Where(t => health >= t.MinHealth && health <= t.MaxHealth)
                .FirstOrDefault();

            return match?.State ?? InjuryState.DISABLED;
        }
    }
}
