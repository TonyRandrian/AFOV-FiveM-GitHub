using metabolism.Server.Application;
using metabolism.Server.Application.Abstractions;
using metabolism.Server.Domain;
using metabolism.Shared.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace metabolism.Server.Infrastructure.Network
{
    public class ClientNotifier : IClientNotifier
    {
        public void SendGauges(PlayerState state)
        {
            List<GaugeStateDto> permanents = new List<GaugeStateDto>();
            List<GaugeStateDto> hiddens = new List<GaugeStateDto>();

            foreach (CharacterGauge gauge in state.Gauges)
            {
                if (gauge.Definition.DisplayType == GaugeDisplayType.PERMANENT)
                {
                    permanents.Add(new GaugeStateDto
                    {
                        Name = gauge.Definition.Name,
                        Value = gauge.Value,
                        Max = gauge.Max
                    });
                } 
                else
                {
                    hiddens.Add(new GaugeStateDto
                    {
                        Name = gauge.Definition.Name,
                        Value = gauge.Value,
                        Max = gauge.Max
                    });
                }
            }

            object response = new 
            {
                Permanents = permanents,
                Hiddens = hiddens
            };

            string responseJson = JsonConvert.SerializeObject(response);
            state.Player.TriggerEvent(Events.UPDATE_STATE, responseJson);
        }
    }
}
