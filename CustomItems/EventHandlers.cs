using HarmonyLib;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Firearms;
using InventorySystem.Items;
using PlayerRoles.FirstPersonControl;
using PluginAPI.Core;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MEC;

namespace CustomItems
{
    internal class EventHandlers
    {
        //patch used for triggering custom items
        [HarmonyPatch(typeof(SingleBulletHitreg), nameof(SingleBulletHitreg.ServerRandomizeRay))]
        public static class PerfectAccuracy
        {
            public static bool Prefix(SingleBulletHitreg __instance)
            {
                Log.Info("hitreg patch");
                if (!EventManager.ExecuteEvent(new PlayerShotWeaponEvent(__instance.Hub, __instance.Firearm)))
                {
                    return false;
                }
                Ray ray = new Ray(__instance.Hub.PlayerCameraReference.position, __instance.Hub.PlayerCameraReference.forward);

                Vector3 randomspread = (new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value) - Vector3.one / 2f).normalized;
                FirearmBaseStats baseStats = __instance.Firearm.BaseStats;
                IFpcRole fpc = (__instance.Hub.roleManager.CurrentRole as IFpcRole);
                float factors = baseStats.GetInaccuracy(__instance.Firearm, __instance.Firearm.AdsModule.ServerAds, fpc.FpcModule.Motor.Velocity.magnitude, fpc.FpcModule.IsGrounded);
                ray.direction = Quaternion.Euler(randomspread * factors) * ray.direction;

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, baseStats.MaxDistance(), StandardHitregBase.HitregMask))
                {
                    __instance.ServerProcessRaycastHit(ray, hit);
                    //CUSTOM ITEM LOGIC HERE WEE WOO
                    ItemBase item = __instance.Firearm;

                    //CUSTOM ITEM LOGIC HERE WEE WOO
                    return false;
                }
                return false;
            }

            public static void Postfix(SingleBulletHitreg __instance)
            {
            }
        }
    }
}
