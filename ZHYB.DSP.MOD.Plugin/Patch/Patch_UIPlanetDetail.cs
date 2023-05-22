using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patch
{
	[HarmonyPatch(typeof(UIPlanetDetail))]
	internal class Patch_UIPlanetDetail
	{
		[HarmonyPostfix]
		[HarmonyPatch("OnPlanetDataSet")]
		public static void PatchOnPlanetDataSet(UIPlanetDetail __instance)
		{
		}
	}
}