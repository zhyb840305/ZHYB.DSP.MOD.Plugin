using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patch
{
	[HarmonyPatch(typeof(StationComponent))]
	public class Patch_StationComponent
	{
		[HarmonyPrefix, HarmonyPatch("CalcTripEnergyCost")]
		public static bool PatchCalcTripEnergyCost(ref long __result)
		{
			if(!ModConfig.CheatMode.Value)
				return true;
			__result=0;
			return false;
		}
	}
}