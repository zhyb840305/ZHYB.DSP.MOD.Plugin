using ABN;

namespace ZHYB.DSP.MOD.Plugin
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
            __instance.buildArea=800f;
            __instance.warpStorage.grids[0].stackSize=int.MaxValue;
            __instance.droneSpeed=1500f;
            __instance.droneCount=256;
            __instance.droneEnergyPerMeter=0;
            __instance.corePowerGen=1*1000*1000*1000;
            __instance.player.SetSandCount(int.MaxValue);

            GameMain.history.localStationExtraStorage=40000;
            GameMain.history.remoteStationExtraStorage=190000;
            GameMain.history.inserterStackCount=16;
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