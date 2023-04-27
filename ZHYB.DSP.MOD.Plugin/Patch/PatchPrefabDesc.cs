namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(PrefabDesc))]
    internal class PatchPrefabDesc
    {
        [HarmonyPostfix]
        [HarmonyPatch("ReadPrefab")]
        public static void PatchReadPrefab(PrefabDesc __instance)
        {
            __instance.genEnergyPerTick*=ModConfig.ConfigPrefabDesc.genEnergyPerTick.Value;
            __instance.powerCoverRadius*=ModConfig.ConfigPrefabDesc.powerCoverRadius.Value;
            __instance.powerConnectDistance*=ModConfig.ConfigPrefabDesc.powerConnectDistance.Value;

            __instance.stationMaxItemCount=10000;
            __instance.stationMaxEnergyAcc*=100;
            __instance.workEnergyPerTick*=100;

        }
    }
}