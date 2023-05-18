namespace ZHYB.DSP.MOD.Plugin.Patch
{
	[HarmonyPatch(typeof(MechaDrone))]
	internal class Patch_MechaDrone
	{
		[HarmonyPrefix]
		[HarmonyPatch("Update")]
		public static bool PatchUpdate(MechaDrone __instance,ref float dt)
		{
			if(!ModConfig.CheatMode.Value)
				return true;
			if(__instance.stage==1)
			{
				dt=1;
				__instance.stage=2;
				__instance.progress=0f;
			}
			return true;
		}
	}
}