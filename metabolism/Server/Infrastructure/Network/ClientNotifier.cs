using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;
using metabolism.Shared;
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

            foreach (CharacterGaugeStatus gauge in state.Gauges)
            {
                if (gauge.Definition.DisplayType == GaugeDisplayTypeEnum.PERMANENT && 
                    gauge.Definition.Name != "PV")
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
            List<object> responseList = new List<object>();

            foreach (BodyPartCharacterStatus bodyPart in state.BodyPartsStatus)
            {
                responseList.Add(new
                {
                    BodyPart = bodyPart.BodyPart.BodyPart.ToString(),
                    bodyPart.Health
                });
            }

            object response = new
            {
                BodyParts = responseList,
                state.Health
            };

            string responseJson = JsonConvert.SerializeObject(response);
            state.Player.TriggerEvent(Events.UPDATE_BODY_PART, responseJson);
        }

        public void SendPlayerDeath(PlayerState state)
        {
            state.Player.TriggerEvent(Events.PLAYER_DIED);
        }

        public void SendRestrictions(PlayerState state, ActionRestriction restrictions)
        {
            state.Player.TriggerEvent(Events.UPDATE_RESTRICTIONS, (int)restrictions);
        }

        public void SendMovementClipset(PlayerState state, string clipset)
        {
            state.Player.TriggerEvent(Events.UPDATE_MOVEMENT_CLIPSET, clipset ?? string.Empty);
        }
    }
}
