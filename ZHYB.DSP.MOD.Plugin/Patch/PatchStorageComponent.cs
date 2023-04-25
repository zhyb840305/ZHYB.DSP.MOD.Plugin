using System;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(StorageComponent),"GetItemCount")]
    internal class PatchGetItemCount
    {
        [HarmonyPostfix]
        [HarmonyPatch("GetItemCount",new Type[] { typeof(int) })]
        private static void Postfix(int itemId,ref int __result)
        {
            if(itemId!=ItemIds.Foundation)
                return;
            __result=1000;
        }
    }
}