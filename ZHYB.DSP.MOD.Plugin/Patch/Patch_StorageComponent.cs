﻿namespace Patch
{
	[HarmonyPatch(typeof(StorageComponent),"GetItemCount")]
	internal class PatchGetItemCount
	{
		[HarmonyPostfix]
		[HarmonyPatch("GetItemCount",new Type[] { typeof(int) })]
		private static void Postfix(int itemId,ref int __result)
		{
			if(!ModConfig.CheatMode.Value)
				return;
			if(itemId!=ItemIds.Foundation)
				return;
			__result=1000;
		}
	}
}