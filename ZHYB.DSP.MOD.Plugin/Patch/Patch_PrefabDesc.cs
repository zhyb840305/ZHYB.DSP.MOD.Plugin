﻿namespace Patch
{
	[HarmonyPatch(typeof(PrefabDesc))]
	internal class Patch_PrefabDesc
	{
		[HarmonyPostfix]
		[HarmonyPatch("ReadPrefab")]
		public static void PatchReadPrefab(PrefabDesc __instance)
		{
			if(!ModConfig.CheatMode.Value)
				return;
			if(__instance.isPowerNode)
			{
				__instance.genEnergyPerTick*=ModConfig.ConfigPrefabDesc.genEnergyPerTick.Value;
				__instance.powerCoverRadius*=ModConfig.ConfigPrefabDesc.powerCoverRadius.Value;
				__instance.powerConnectDistance*=ModConfig.ConfigPrefabDesc.powerConnectDistance.Value;
			}

			//物流塔
			if(__instance.isStation&&!__instance.isCollectStation)
			{
				__instance.stationMaxItemCount=0;
				__instance.idleEnergyPerTick=0;

				if(__instance.isCollectStation)
				{
					__instance.stationMaxEnergyAcc*=1000;
					__instance.workEnergyPerTick*=1000;
				}
				else
				{
					__instance.stationMaxEnergyAcc*=1000;
					__instance.workEnergyPerTick*=1000;
				}
			}

			if(__instance.isInserter)
			{
				__instance.idleEnergyPerTick=0;
			}

			//超级电池
			if(__instance.isAccumulator)
			{
				__instance.inputEnergyPerTick=( long )1000*1000*1000*1000;
				__instance.outputEnergyPerTick=( long )1000*1000*1000*1000;
				__instance.maxAcuEnergy=( long )1000*1000*1000*1000*1000*1000;
			}
			if(__instance.isPowerExchanger)
			{
				__instance.exchangeEnergyPerTick*=1000;
			}
		}
	}
}