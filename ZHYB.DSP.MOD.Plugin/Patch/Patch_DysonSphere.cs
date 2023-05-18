namespace Patch
{
	[HarmonyPatch(typeof(DysonSphere))]
	public static class Patch_DysonSphere
	{
		[HarmonyPostfix]
		[HarmonyPatch("Init")]
		public static void Postfix(DysonSphere __instance)
		{
			if(!ModConfig.CheatMode.Value)
				return;
			__instance.maxOrbitRadius*=ModConfig.ConfigDysonSphere.maxOrbitRadius.Value;
		}
	}
}