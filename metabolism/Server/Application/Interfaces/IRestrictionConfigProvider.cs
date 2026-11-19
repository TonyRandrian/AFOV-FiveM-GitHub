using metabolism.Server.Domain;
using System.Collections.Generic;

namespace metabolism.Server.Application.Interfaces
{
    public interface IRestrictionConfigProvider
    {
        IReadOnlyList<RestrictionRule> LoadRestrictionRules();
    }
}
