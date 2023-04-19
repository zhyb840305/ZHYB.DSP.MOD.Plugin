using HarmonyLib;

using UnityEngine;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(PowerSystem))]
    internal class PatchPowerSystem
    {
        [HarmonyPostfix]
        [HarmonyPatch("NewGeneratorComponent")]
        public static void PatchNewGeneratorComponent(PowerSystem __instance,int __result)
        {
            ModPlugin.logger.LogInfo("NewGeneratorComponent");
        }

        [HarmonyPostfix]
        [HarmonyPatch("NewAccumulatorComponent")]
        public static void PatchNewAccumulatorComponent(PowerSystem __instance,int __result)
        {
            ModPlugin.logger.LogInfo("NewAccumulatorComponent");
        }

        [HarmonyPostfix]
        [HarmonyPatch("NewConsumerComponent")]
        public static void PatchNewConsumerComponent(PowerSystem __instance,int __result)
        {
            ModPlugin.logger.LogInfo("NewConsumerComponent");
        }

        [HarmonyPostfix]
        [HarmonyPatch("NewExchangerComponent")]
        public static void PatchNewExchangerComponent(PowerSystem __instance,int __result)
        {
            ModPlugin.logger.LogInfo("NewExchangerComponent");
        }
    }
}