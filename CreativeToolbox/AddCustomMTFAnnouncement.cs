using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(NineTailedFoxAnnouncer), nameof(NineTailedFoxAnnouncer.AnnounceNtfEntrance))]
    class AddCustomMTFAnnouncement
    {
        public static bool Prefix(NineTailedFoxAnnouncer __instance, int _scpsLeft, int _mtfNumber, char _mtfLetter)
        {
			string text = string.Empty;
			string[] array = new string[]
			{
				_mtfNumber.ToString("00")[0].ToString(),
				_mtfNumber.ToString("00")[1].ToString()
			};
			if (ClutterSpawner.IsHolidayActive(Holidays.Christmas))
			{
				text += "XMAS_EPSILON11 ";
				text = text + "NATO_" + _mtfLetter.ToString() + " ";
				text = text + array[0] + array[1] + " ";
				text = string.Concat(new object[]
				{
					text,
					"XMAS_HASENTERED ",
					_scpsLeft,
					" XMAS_SCPSUBJECTS"
				});
			}
			else
			{
				text += "MTFUNIT EPSILON 11 DESIGNATED ";
				text = text + "NATO_" + _mtfLetter.ToString() + " ";
				text = text + array[0] + array[1] + " ";
				text += "HASENTERED ALLREMAINING ";
				text += ((_scpsLeft <= 0) ? "NOSCPSLEFT" : ("AWAITINGRECONTAINMENT " + _scpsLeft + ((_scpsLeft == 1) ? " SCPSUBJECT" : " SCPSUBJECTS")));
			}
			float num = (global::AlphaWarheadController.Host.timeToDetonation <= 0f) ? 2.5f : 1f;
			if (CreativeToolbox.ConfigRef.Config.EnableCustomAnnouncements)
				__instance.ServerOnlyAddGlitchyPhrase(CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncement, UnityEngine.Random.Range(0.08f, 0.1f) * num, UnityEngine.Random.Range(0.07f, 0.09f) * num);
			else
				__instance.ServerOnlyAddGlitchyPhrase(text, UnityEngine.Random.Range(0.08f, 0.1f) * num, UnityEngine.Random.Range(0.07f, 0.09f) * num);
			return false;
		}
    }
}
