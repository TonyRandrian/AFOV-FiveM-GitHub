using CitizenFX.Core;
using metabolism.Client.Services;
using metabolism.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client
{
    public class ClientMain : BaseScript
    {
        private bool _isHudVisible = false;
        private bool _isAllHudVisible = true;
        private PlayerActivity _lastActivity = PlayerActivity.None;
        private readonly Random _random = new Random();

        public ClientMain()
        {
            EventHandlers[Events.UPDATE_STATE] +=
                new Action<string>(OnStateUpdated);
            EventHandlers[Events.GAME_EVENT_TRIGGERED] +=
                new Action<string, List<object>>(OnGameEventTriggered);
            EventHandlers[Events.UPDATE_BODY_PART] += 
                new Action<string>(OnBodyPartUpdated);
            EventHandlers["playerSpawned"] +=
                new Action<IDictionary<string, object>>(OnPlayerSpawned);

            RegisterCommand("tp", new Action(TeleportRandomly), false);

            // Commande pour remplir la barre de vie
            RegisterCommand("hp", new Action(FillHealth), false);
        }

        private async void OnPlayerSpawned(IDictionary<string, object> infos)
        {
            await Delay(500);
        }

        private void OnBodyPartUpdated(string bodyPartJson)
        {
            SendNuiMessage("{\"action\":\"updateBodyParts\", \"value\":" + bodyPartJson + "}");
        }

        private void OnStateUpdated(string gaugesJson)
        {
            SendNuiMessage("{\"action\":\"updateState\", \"value\":" + gaugesJson + "}");
        }

        private void OnGameEventTriggered(string eventName, List<object> args)
        {
            if (eventName == "CEventNetworkEntityDamage")
            {
                int victimHandle = Convert.ToInt32(args[0]);

                if (DoesEntityExist(victimHandle) && IsEntityAPed(victimHandle))
                {
                    if (victimHandle == Game.PlayerPed.Handle)
                    {
                        int boneId = 0;
                        bool success = GetPedLastDamageBone(victimHandle, ref boneId);
                        ClearPedLastDamageBone(victimHandle);

                        int damageRaw = Convert.ToInt32(args[2]);
                        float damageAmount = BitConverter.ToSingle(BitConverter.GetBytes(damageRaw), 0);
                        DamageSource source = DamageService.GetDamageSource(args);

                        if (success && boneId != 0)
                        {
                            TriggerServerEvent(Events.PLAYER_DAMAGED, boneId, damageAmount, source);
                            Debug.WriteLine($"{boneId} : {damageAmount}");
                        }
                    }
                }
            }
        }

        [Tick]
        public Task OnHudTick()
        {
            bool shouldShow = !(IsPauseMenuActive() 
                || IsWarningMessageActive() 
                || IsCutsceneActive() 
                || GetIsLoadingScreenActive());

            if (shouldShow != _isAllHudVisible)
            {
                SetAllHudVisible(shouldShow);
            }
            
            if (IsControlJustPressed(0, 311))
            {
                SetHudVisible(!_isHudVisible);
            }

            return Task.FromResult(0);
        }

        [Tick]
        public async Task OnActivityTick()
        {
            await Delay(100);

            int ped = PlayerPedId();
            PlayerActivity current = PlayerActivity.None;

            if (IsPedSwimmingUnderWater(ped))
            {
                current |= PlayerActivity.Underwater;
            }
            if (IsPedSprinting(ped))
            {
                current |= PlayerActivity.Sprinting;
            }

            if (current != _lastActivity)
            {
                _lastActivity = current;
                TriggerServerEvent(Events.PLAYER_ACTIVITY_CHANGED, (int)current);
            }
        }

        private void SetHudVisible(bool visible)
        {
            _isHudVisible = visible;
            string json = "{\"action\":\"setHudVisible\",\"value\":" + (visible ? "true" : "false") + "}";
            SendNuiMessage(json);
        }

        private void SetAllHudVisible(bool visible)
        {
            _isAllHudVisible = visible;
            string json = "{\"action\":\"setAllHudVisible\",\"value\":" + (visible ? "true" : "false") + "}";
            SendNuiMessage(json);
        }

        private void TeleportRandomly()
        {
            Vector3[] locations =
            {
            new Vector3(215.76f, -810.12f, 30.73f),
            new Vector3(-1037.74f, -2737.82f, 20.17f),
            new Vector3(1854.12f, 3686.45f, 34.27f),
            new Vector3(-425.12f, 1123.45f, 325.85f),
            new Vector3(1692.62f, 4823.10f, 42.06f)
        };

            int index = _random.Next(locations.Length);
            Vector3 destination = locations[index];

            int playerPed = PlayerPedId();

            SetEntityCoords(
                playerPed,
                destination.X,
                destination.Y,
                destination.Z,
                false,
                false,
                false,
                false
            );

            Debug.WriteLine($"[Metabolism] Teleported to: {destination}");
        }

        private void FillHealth()
        {
            int playerPed = PlayerPedId();

            int maxHealth = GetEntityMaxHealth(playerPed);

            SetEntityHealth(playerPed, maxHealth);

            Debug.WriteLine("[Metabolism] Health restored!");
        }
    }
}
