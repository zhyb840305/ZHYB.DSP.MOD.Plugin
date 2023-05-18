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

	[HarmonyPatch(typeof(GameHistoryData))]
	internal class Patch_GameHistoryData
	{
		[HarmonyPostfix, HarmonyPatch("Init"), HarmonyPatch("SetForNewGame"), HarmonyPatch("Import")]
		public static void PatchGameHistoryData(GameHistoryData __instance)
		{
			if(!ModConfig.CheatMode.Value)
				return;
			__instance.localStationExtraStorage=100000-5000;
			__instance.remoteStationExtraStorage=500000-10000;
			__instance.logisticShipCarries=5000;
			__instance.logisticDroneCarries=500;
			__instance.logisticCourierCarries=100;
			__instance.stationPilerLevel=16;
		}
	}

	[HarmonyPatch(typeof(Mecha))]
	internal class Patch_Mecha
	{
		public static bool StackSizeUpdated = false;

		[HarmonyPostfix, HarmonyPatch("Init"), HarmonyPatch("SetForNewGame"), HarmonyPatch("Import")]
		public static void PostfixPostfix(Mecha __instance)
		{
			PatchMecha(__instance);
		}

		private static void PatchMecha(Mecha __instance)
		{
			if(!ModConfig.CheatMode.Value)
				return;

			__instance.buildArea=1000f;
			__instance.warpStorage.grids[0].stackSize=int.MaxValue;
			__instance.droneSpeed=150f;
			__instance.droneCount=256;
			__instance.droneMovement=4;
			__instance.droneEnergyPerMeter=0;
			__instance.corePowerGen=1*1000*1000*1000;
			__instance.player.SetSandCount(int.MaxValue);

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