using metabolism.Server.Application;
using metabolism.Server.Application.Abstractions;
using metabolism.Shared.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace metabolism.Server.Infrastructure.Network
{
    public class ClientNotifier : IClientNotifier
    {
        public void SendGauges(PlayerState state)
        {
            List<GaugeStateDto> dtos = state.Gauges
                .Select(g => new GaugeStateDto
                {
                    Name = g.Definition.Name,
                    Value = g.Value,
                    Max = g.Max
                })
                .ToList();

            state.Player.TriggerEvent(Events.UPDATE_STATE, JsonConvert.SerializeObject(dtos));
        }
    }
}
