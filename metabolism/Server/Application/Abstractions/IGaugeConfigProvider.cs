using metabolism.Server.Domain;
using System.Collections.Generic;

namespace metabolism.Server.Application.Abstractions
{
    public interface IGaugeConfigProvider
    {
        IReadOnlyList<GaugeDefinition> LoadGauges();
    }
}
