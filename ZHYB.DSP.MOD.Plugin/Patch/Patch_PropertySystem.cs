namespace ZHYB.DSP.MOD.Plugin
{
    [HarmonyPatch(typeof(PropertySystem))]
    public class Patch_PropertySystem
    {
        [HarmonyPostfix]
        [HarmonyPatch("GetItemTotalProperty")]
        public static void Postfix(ref int __result)
        {
            __result=int.MaxValue;
        }
    }
}