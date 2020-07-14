using System;
using HarmonyLib;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(MTFRespawn), nameof(MTFRespawn.CallRpcAnnouncCI))]
    class AddCustomCIAnnouncement
    {
        public static void Prefix(MTFRespawn __instance)
        {
            if (CreativeToolbox.ConfigRef.Config.EnableCustomAnnouncements && !String.IsNullOrWhiteSpace(CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncement))
                PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement($"{CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncement}", false, false);
        }
    }
}
