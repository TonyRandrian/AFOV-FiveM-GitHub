using metabolism.Server.Domain;
using System.Collections.Generic;

namespace metabolism.Server.Application.Interfaces
{
    public interface IGaugeConfigProvider
    {
        IReadOnlyList<GaugeDefinition> LoadGauges();
    }
}
