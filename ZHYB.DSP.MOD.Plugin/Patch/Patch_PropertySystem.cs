namespace Patch
{
	[HarmonyPatch(typeof(PropertySystem))]
	public class Patch_PropertySystem
	{
		[HarmonyPostfix]
		[HarmonyPatch("GetItemTotalProperty")]
		public static void Postfix(ref int __result)
		{
			if(!ModConfig.CheatMode.Value)
				return;
			__result=int.MaxValue;
		}
	}
}