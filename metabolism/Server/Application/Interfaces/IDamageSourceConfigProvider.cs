using metabolism.Server.Domain;
using System.Collections.Generic;

namespace metabolism.Server.Application.Interfaces
{
    public interface IDamageSourceConfigProvider
    {
        IReadOnlyList<DamageSourceProfile> LoadDamageSourceProfiles();
    }
}
