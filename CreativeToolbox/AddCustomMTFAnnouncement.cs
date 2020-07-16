using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Exiled.API.Features;
using HarmonyLib;
using Respawning.NamingRules;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(NineTailedFoxNamingRule), nameof(NineTailedFoxNamingRule.PlayEntranceAnnouncement))]
    class AddCustomMTFAnnouncement
    {
        public static bool Prefix(NineTailedFoxNamingRule __instance, string regular)
        {
			string cassieUnitName = __instance.GetCassieUnitName(regular);
			int num = (from x in global::ReferenceHub.GetAllHubs().Values
					   where x.characterClassManager.CurRole.team == global::Team.SCP && x.characterClassManager.CurClass != global::RoleType.Scp0492
					   select x).Count<global::ReferenceHub>();
			StringBuilder stringBuilder = global::StringBuilderPool.Rent();
			if (global::ClutterSpawner.IsHolidayActive(global::Holidays.Christmas))
			{
				stringBuilder.Append("XMAS_EPSILON11 ");
				stringBuilder.Append(cassieUnitName);
				stringBuilder.Append("XMAS_HASENTERED ");
				stringBuilder.Append(num);
				stringBuilder.Append(" XMAS_SCPSUBJECTS");
			}
			else
			{
				stringBuilder.Append("MTFUNIT EPSILON 11 DESIGNATED ");
				stringBuilder.Append(cassieUnitName);
				stringBuilder.Append(" HASENTERED ALLREMAINING ");
				if (num == 0)
				{
					stringBuilder.Append("NOSCPSLEFT");
				}
				else
				{
					stringBuilder.Append("AWAITINGRECONTAINMENT ");
					stringBuilder.Append(num);
					if (num == 1)
					{
						stringBuilder.Append(" SCPSUBJECT");
					}
					else
					{
						stringBuilder.Append(" SCPSUBJECTS");
					}
				}
			}
			if (CreativeToolbox.ConfigRef.Config.EnableCustomAnnouncements)
			{
				stringBuilder.Clear();
				stringBuilder.Append(CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncement);
				__instance.ConfirmAnnouncement(ref stringBuilder);
			}
			else
				__instance.ConfirmAnnouncement(ref stringBuilder);
			StringBuilderPool.Return(stringBuilder);
			return false;
		}
    }
}
