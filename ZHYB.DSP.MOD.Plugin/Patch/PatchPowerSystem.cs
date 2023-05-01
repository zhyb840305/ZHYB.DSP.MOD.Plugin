namespace Patch
{
    [HarmonyPatch(typeof(PowerSystem))]
    internal class PatchPowerSystem
    {
        [HarmonyPostfix]
        [HarmonyPatch("NewGeneratorComponent")]
        public static void PatchNewGeneratorComponent(PowerSystem __instance,int __result)
        {
        }

        [HarmonyPostfix]
        [HarmonyPatch("NewAccumulatorComponent")]
        public static void PatchNewAccumulatorComponent(PowerSystem __instance,int __result)
        {
            __instance.accPool[__result].curEnergy=__instance.accPool[__result].maxEnergy;
        }

        [HarmonyPostfix]
        [HarmonyPatch("NewConsumerComponent")]
        public static void PatchNewConsumerComponent(PowerSystem __instance,int __result)
        {
        }

        [HarmonyPostfix]
        [HarmonyPatch("NewExchangerComponent")]
        public static void PatchNewExchangerComponent(PowerSystem __instance,int __result)
        {
        }
    }
}