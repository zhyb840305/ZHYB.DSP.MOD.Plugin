namespace Patch
{
	[HarmonyPatch(typeof(DysonSphere))]
	public static class Patch_DysonSphere
	{
		[HarmonyPostfix]
		[HarmonyPatch("Init")]
		public static void Postfix(DysonSphere __instance)
		{
			__instance.maxOrbitRadius*=ModConfig.ConfigDysonSphere.maxOrbitRadius.Value;
		}
	}
}