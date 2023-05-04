namespace Patch
{
    [HarmonyPatch(typeof(PrefabDesc))]
    internal class PatchPrefabDesc
    {
        [HarmonyPostfix]
        [HarmonyPatch("ReadPrefab")]
        public static void PatchReadPrefab(PrefabDesc __instance)
        {
            if(__instance.isPowerNode)
            {
                __instance.genEnergyPerTick*=ModConfig.ConfigPrefabDesc.genEnergyPerTick.Value;
                __instance.powerCoverRadius*=ModConfig.ConfigPrefabDesc.powerCoverRadius.Value;
                __instance.powerConnectDistance*=ModConfig.ConfigPrefabDesc.powerConnectDistance.Value;
            }
            //物流塔
            if(__instance.isStation&&!__instance.isCollectStation)
            {
                __instance.stationMaxItemCount=10000;
                __instance.stationMaxEnergyAcc*=100;
                __instance.workEnergyPerTick*=100;
            }

            //超级电池
            if(__instance.isAccumulator)
            {
                var MyLong=( long )1000*1000*1000*1000*1000*1000;
                __instance.inputEnergyPerTick=( long )1000*1000*1000*1000;
                __instance.outputEnergyPerTick=( long )1000*1000*1000*1000;
                __instance.maxAcuEnergy=MyLong;
            }
        }
    }
}