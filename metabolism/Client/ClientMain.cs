using CitizenFX.Core;
using metabolism.Client.Services;
using metabolism.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client
{
    public class ClientMain : BaseScript
    {
        int lastIndex = 0;
        private bool _isHudVisible = false;
        private bool _isAllHudVisible = true;
        private PlayerActivity _lastActivity = PlayerActivity.None;
        private ActionRestriction _restrictions = ActionRestriction.NONE;
        private string _currentClipset = null;
        

        public ClientMain()
        {
            EventHandlers[Events.UPDATE_STATE] +=
                new Action<string>(OnStateUpdated);
            EventHandlers[Events.GAME_EVENT_TRIGGERED] +=
                new Action<string, List<object>>(OnGameEventTriggered);
            EventHandlers[Events.UPDATE_BODY_PART] +=
                new Action<string>(OnBodyPartUpdated);
            EventHandlers[Events.PLAYER_SPAWNED] +=
                new Action<IDictionary<string, object>>(OnPlayerSpawned);
            EventHandlers[Events.PLAYER_DIED] +=
                new Action(OnPlayerDied);
            EventHandlers[Events.UPDATE_RESTRICTIONS] +=
                new Action<int>(OnRestrictionsUpdated);
            EventHandlers[Events.UPDATE_MOVEMENT_CLIPSET] +=
                new Action<string>(OnMovementClipsetUpdated);

            RegisterCommand("tp", new Action(TeleportRandomly), false);

            // Commande pour remplir la barre de vie
            RegisterCommand("hp", new Action(FillHealth), false);

            RegisterCommand("weapon", new Action(() =>
            {
                int ped = Game.PlayerPed.Handle;

                if (DoesEntityExist(ped))
                {
                    // Hash de l'arme (ici un Molotov pour tester le feu, modifiable selon vos envies)
                    uint molotovHash = (uint)GetHashKey("WEAPON_MOLOTOV");
                    uint pistol_1 = (uint)GetHashKey("WEAPON_COMBATPISTOL");
                    uint pistol_2 = (uint)GetHashKey("WEAPON_APPISTOL");

                    // Donner l'arme au joueur avec 10 munitions
                    GiveWeaponToPed(ped, molotovHash, 10, false, false);
                    GiveWeaponToPed(ped, pistol_1, 100, false, false);
                    GiveWeaponToPed(ped, pistol_2, 100, false, false);
                }
            }), false);
        }

        private void EnablePvP()
        {
            NetworkSetFriendlyFireOption(true);
            SetCanAttackFriendly(PlayerPedId(), true, false);
        }

        private void OnRestrictionsUpdated(int restrictions)
        {
            _restrictions = (ActionRestriction)restrictions;
        }

        private async void OnMovementClipsetUpdated(string clipset)
        {
            if (clipset == _currentClipset)
                return;

            int ped = PlayerPedId();

            if (string.IsNullOrEmpty(clipset))
            {
                ResetPedMovementClipset(ped, 0f);
            }
            else
            {
                RequestClipSet(clipset);

                int attempts = 0;
                while (!HasClipSetLoaded(clipset) && attempts < 100)
                {
                    await Delay(10);
                    attempts++;
                }

                if (HasClipSetLoaded(clipset))
                {
                    SetPedMovementClipset(ped, clipset, 1f);
                }
            }

            _currentClipset = clipset;
        }

        private void OnPlayerDied()
        {
            int playerPed = Game.PlayerPed.Handle;
            SetEntityHealth(playerPed, 0);
            Debug.WriteLine("[Metabolism] Player died.");
        }

        private async void OnPlayerSpawned(IDictionary<string, object> infos)
        {
            await Delay(500);

            EnablePvP();
            int ped = Game.PlayerPed.Handle;

            SetEntityMaxHealth(ped, 10000);
            SetEntityHealth(ped, 10000);
            SetPedSuffersCriticalHits(ped, true);

            TriggerServerEvent(Events.PLAYER_RESPAWNED);
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
                        SetEntityHealth(victimHandle, 10000);
                        int boneId = 0;
                        GetPedLastDamageBone(victimHandle, ref boneId);
                        ClearPedLastDamageBone(victimHandle);

                        int damageRaw = Convert.ToInt32(args[2]);
                        float damageAmount = BitConverter.ToSingle(BitConverter.GetBytes(damageRaw), 0);
                        DamageSource source = DamageService.GetDamageSource(args);

                        if (damageAmount > 0)
                        {
                            TriggerServerEvent(Events.PLAYER_DAMAGED, boneId, damageAmount, (int)source);
                            ClearEntityLastDamageEntity(victimHandle);
                            ClearEntityLastWeaponDamage(victimHandle);
                        }
                    }
                }
            }
        }

        [Tick]
        public Task OnRestrictionsTick()
        {
            int ped = PlayerPedId();

            if (_restrictions.HasFlag(ActionRestriction.NO_SPRINT))
            {
                DisableControlAction(0, 21, true);
            }

            if (_restrictions.HasFlag(ActionRestriction.NO_JUMP))
                DisableControlAction(0, 22, true);

            if (_restrictions.HasFlag(ActionRestriction.NO_WALK))
            {
                DisableControlAction(0, 30, true);
                DisableControlAction(0, 31, true);
            }

            if (_restrictions.HasFlag(ActionRestriction.NO_AIM))
                DisableControlAction(0, 25, true);

            if (_restrictions.HasFlag(ActionRestriction.NO_FIRE))
            {
                DisableControlAction(0, 24, true);
                DisableControlAction(0, 257, true);
            }

            if (_restrictions.HasFlag(ActionRestriction.SLOW_MOVEMENT))
            {
                SetPedMoveRateOverride(ped, 0.75f);
            }

            return Task.FromResult(0);
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

            Vector3 destination = locations[lastIndex];
            lastIndex = (lastIndex + 1) % locations.Length;
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
