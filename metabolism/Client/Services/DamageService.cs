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

            int attackerHandler = Convert.ToInt32(eventArgs[1]);
            uint weaponHash = Convert.ToUInt32(eventArgs[7]);

            if (weaponHash == Hash("WEAPON_FALL"))
                return DamageSource.FALL;

            if (weaponHash == Hash("WEAPON_RUN_OVER_BY_CAR")
                || weaponHash == Hash("WEAPON_RAMMED_BY_CAR"))
                return DamageSource.VEHICLE_COLLISION;

            if (weaponHash == Hash("WEAPON_EXPLOSION"))
                return DamageSource.EXPLOSION;

            // Détection du feu, du Molotov ou de tout dégât classé comme "Fire" (type 5) par le moteur
            if (weaponHash == Hash("WEAPON_FIRE")
                || weaponHash == Hash("WEAPON_MOLOTOV")
                || GetWeaponDamageType(weaponHash) == 5)
                return DamageSource.FIRE;

            if (weaponHash == Hash("WEAPON_DROWNING"))
                return DamageSource.DROWNING;

            uint group = (uint)GetWeapontypeGroup(weaponHash);
            if (group == Hash("GROUP_MELEE"))
                return DamageSource.MELEE_WEAPON;

            if (group == Hash("GROUP_UNARMED"))
                return DamageSource.PUNCH_UNARMED;

            if (FirearmGroups.Contains(group))
                return DamageSource.FIREARM;

            if (attackerHandler > 0 && DoesEntityExist(attackerHandler)
                && IsEntityAVehicle(attackerHandler))
                return DamageSource.VEHICLE_COLLISION;

            if (attackerHandler <= 0)
                return DamageSource.ENVIRONMENT;

            return DamageSource.UNKNOWN;
        }

        private static uint Hash(string name)
        {
            return unchecked((uint)GetHashKey(name));
        }
    }
}