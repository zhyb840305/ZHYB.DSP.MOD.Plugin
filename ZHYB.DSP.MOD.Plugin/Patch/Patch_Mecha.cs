using ABN;

namespace Patch
{
	[HarmonyPatch(typeof(GameAbnormalityData_0925))]
	internal class Patch_GameAbnormalityData_0925
	{
		[HarmonyPrefix]
		[HarmonyPatch("NothingAbnormal")]
		public static bool IsGameLegitimate(ref bool __result)
		{
			__result=true;
			return false;
		}
	}

	[HarmonyPatch(typeof(Mecha))]
	internal class Patch_Mecha
	{
		public static bool StackSizeUpdated = false;

		[HarmonyPostfix]
		[HarmonyPatch("Init")]
		[HarmonyPatch("SetForNewGame")]
		[HarmonyPatch("Import")]
		[HarmonyPatch("Export")]
		public static void PostfixPostfix(Mecha __instance)
		{
			PatchMecha(__instance);
		}

		private static void PatchMecha(Mecha __instance)
		{
			__instance.buildArea=1000f;
			__instance.warpStorage.grids[0].stackSize=int.MaxValue;
			__instance.droneSpeed=150f;
			__instance.droneCount=256;
			__instance.droneMovement=4;
			__instance.droneEnergyPerMeter=0;
			__instance.corePowerGen=1*1000*1000*1000;
			__instance.player.SetSandCount(int.MaxValue);

			GameMain.history.localStationExtraStorage=100000;
			GameMain.history.remoteStationExtraStorage=500000;
			GameMain.history.stationPilerLevel=16;

			if(StackSizeUpdated)
				return;

			foreach(ItemProto data in LDB.items.dataArray)
			{
				int num = data.StackSize * 10000;
				data.StackSize=num;
				StorageComponent.itemStackCount[data.ID]=num;
			}

			StackSizeUpdated=true;
		}
	}
}