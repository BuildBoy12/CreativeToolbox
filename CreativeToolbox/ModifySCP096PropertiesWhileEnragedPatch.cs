using System;
using HarmonyLib;
using PlayableScps;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.Enrage))]
    class ModifySCP096PropertiesWhileEnragedPatch
    {
        public static bool Prefix(Scp096 __instance)
        {
            if (!__instance.Enraged)
            {
                __instance.AddReset();
                return true;
            }
            if (CreativeConfig.EnableSCP096StatsModify)
            {
                __instance.SetMovementSpeed(CreativeConfig.SCP096EnrageMoveSpeed);
                __instance.SetJumpHeight(CreativeConfig.SCP096EnrageJumpHeight);
            }
            else
            {
                __instance.SetMovementSpeed(12f);
                __instance.SetJumpHeight(10f);
            }
            __instance.PlayerState = Scp096PlayerState.Enraged;
            __instance.EnrageTimeLeft = __instance.EnrageTime;
            return false;
        }
    }
}
