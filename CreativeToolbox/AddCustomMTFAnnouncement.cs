using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(NineTailedFoxAnnouncer), nameof(NineTailedFoxAnnouncer.ServerOnlyAddGlitchyPhrase))]
    class AddCustomMTFAnnouncement
    {
        public static bool Prefix(NineTailedFoxAnnouncer __instance, string tts, float glitchChance, float jamChance)
        {
            if (CreativeConfig.EnableCustomAnnouncement && !String.IsNullOrWhiteSpace(CreativeConfig.MTFAnnouncement))
            {
                PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement($"{CreativeConfig.MTFAnnouncement}", false, true);
                return false;
            }
            return true;
        }
    }
}
