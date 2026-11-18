using CitizenFX.Core;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client.Services
{
    public static class DamageService
    {
        private static readonly HashSet<uint> FirearmGroups = new HashSet<uint>
        {
            Hash("GROUP_PISTOL"),
            Hash("GROUP_SMG"),
            Hash("GROUP_RIFLE"),
            Hash("GROUP_MG"),
            Hash("GROUP_SHOTGUN"),
            Hash("GROUP_SNIPER"),
            Hash("GROUP_HEAVY")
        };

        public static DamageSource GetDamageSource(List<object> eventArgs)
        {
            if (eventArgs == null || eventArgs.Count <= 7)
                return DamageSource.UNKNOWN;

            int victimPed = Convert.ToInt32(eventArgs[0]);
            int attackerPed = Convert.ToInt32(eventArgs[1]);
            uint weaponHash = unchecked(
                                (uint)Convert.ToInt32(eventArgs[6])
                              );

            Debug.WriteLine($"Victim : {victimPed}");
            Debug.WriteLine($"Attacker : {attackerPed}");

            if ((weaponHash == Hash("WEAPON_DROWNING")
                || weaponHash == Hash("WEAPON_DROWNING_IN_VEHICLE")
                || IsPlayerDrowning(victimPed)) && attackerPed == -1)
                return DamageSource.DROWNING;

            if (weaponHash == Hash("WEAPON_FALL"))
                return DamageSource.FALL;

            if (weaponHash == Hash("WEAPON_RUN_OVER_BY_CAR")
                || weaponHash == Hash("WEAPON_RAMMED_BY_CAR"))
                return DamageSource.VEHICLE_COLLISION;

            if (weaponHash == Hash("WEAPON_EXPLOSION"))
                return DamageSource.EXPLOSION;

            if (weaponHash == Hash("WEAPON_FIRE")
                || weaponHash == Hash("WEAPON_MOLOTOV"))
                return DamageSource.FIRE;

            uint group = (uint)GetWeapontypeGroup(weaponHash);
            if (group == Hash("GROUP_MELEE"))
                return DamageSource.MELEE_WEAPON;

            if (group == Hash("GROUP_UNARMED"))
                return DamageSource.PUNCH_UNARMED;

            if (FirearmGroups.Contains(group))
                return DamageSource.FIREARM;

            if (attackerPed > 0 && DoesEntityExist(attackerPed)
                && IsEntityAVehicle(attackerPed))
                return DamageSource.VEHICLE_COLLISION;

            if (attackerPed <= 0)
                return DamageSource.ENVIRONMENT;

            return DamageSource.UNKNOWN;
        }

        private static uint Hash(string name)
        {
            return unchecked((uint)GetHashKey(name));
        }

        private static bool IsPlayerDrowning(int ped)
        {
            int playerId = NetworkGetPlayerIndexFromPed(ped);

            if (playerId == -1)
                return false;

            float airRemaining = GetPlayerUnderwaterTimeRemaining(playerId);

            if (airRemaining > 0.0f)
                return false;

            if (IsPedSwimmingUnderWater(ped))
                return true;

            // Véhicule immergé
            if (IsPedInAnyVehicle(ped, false))
            {
                int vehicle = GetVehiclePedIsIn(ped, false);

                if (GetEntitySubmergedLevel(vehicle) > 0.7f)
                    return true;
            }

            return false;
        }
    }
}