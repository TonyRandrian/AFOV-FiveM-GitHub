using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Utils;
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
                if (gauge.Definition.DisplayType == GaugeDisplayTypeEnum.PERMANENT)
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
        
        public void SendBodyParts(PlayerState state)
        {
            List<object> response = new List<object>();

            foreach (BodyPartCharacterStatus bodyPart in state.BodyPartsStatus)
            {
                response.Add(new
                {
                    BodyPart = bodyPart.BodyPart.BodyPart.ToString(),
                    bodyPart.Health
                });
            }

            string responseJson = JsonConvert.SerializeObject(response);
            state.Player.TriggerEvent(Events.UPDATE_BODY_PART, responseJson);
        }
    }
}
