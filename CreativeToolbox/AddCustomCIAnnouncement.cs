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
            if (CreativeConfig.EnableCustomAnnouncement && !String.IsNullOrWhiteSpace(CreativeConfig.MTFAnnouncement))
                PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement($"{CreativeConfig.CIAnnouncement}", false, false);
        }
    }
}
