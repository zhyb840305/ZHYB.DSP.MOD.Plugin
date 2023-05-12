using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Experimental.PlayerLoop;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
	[HarmonyPatch(typeof(MechaDrone))]
	internal class Patch_MechaDrone
	{
		[HarmonyPrefix]
		[HarmonyPatch("Update")]
		public static bool PatchUpdate(MechaDrone __instance,ref float dt)
		{
			if(__instance.stage==1)
			{
				dt=1;
				__instance.progress=0f;
			}
			return true;
		}
	}
}