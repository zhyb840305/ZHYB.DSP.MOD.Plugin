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

		[HarmonyPrefix, HarmonyPatch("SetPCState")]
		public static bool PatchSetPCState(StationComponent __instance)
		{
			__instance.energy=__instance.energyMax;

			return true;
		}
	}
}