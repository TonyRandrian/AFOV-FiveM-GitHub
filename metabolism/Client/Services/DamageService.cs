using CitizenFX.Core;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;


namespace metabolism.Client.Services
{
    public class DamageService
    {

        public static void GetDamageSource(List<object> eventArgs)
        {
            int attackerHandler = Convert.ToInt32(eventArgs[1]);
            uint weaponHash = Convert.ToUInt32(eventArgs[7]);

            DamageSource source = DamageSource.UNKNOWN;

            if (attackerHandler == -1 || attackerHandler == 0)
            {
                if (weaponHash == (uint)GetHashKey("WEAPON_FALL"))
                {
                    source = DamageSource.FALL;
                }
                else
                {
                    source = DamageSource.ENVIRONMENT;
                }
            }
            else
            {
                if (IsEntityAVehicle(attackerHandler))
                {
                    source = DamageSource.VEHICLE_COLLISION;
                }
                else if (IsEntityAPed(attackerHandler))
                {
                    if (weaponHash == (uint)GetHashKey("WEAPON_UNARMED"))
                    {
                        source = DamageSource.PUNCH_UNARMED;
                    }
                    else if (Weapon)
                    {

                    }
                    else
                    {
                        source = DamageSource.FIREARM
                    }
                }
            }

        }
    }
}
