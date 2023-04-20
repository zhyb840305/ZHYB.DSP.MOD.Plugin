using HarmonyLib;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(PlanetTransport))]
    internal class PatchPlanetTransport
    {
        [HarmonyPostfix]
        [HarmonyPatch("NewStationComponent")]
        public static void PatchNewStationComponent(PlanetTransport __instance,StationComponent __result)
        {
            StationComponent component = __result;
            if(component.isVeinCollector)
            {
                __instance.factory.factorySystem.minerPool[component.minerId].speed=10*1000;
                return;
            }
            if(component.isCollector)
                return;
            PlanetTransport planetTransport = __instance;
            PrefabDesc prefabDesc = ((ProtoSet<ItemProto>) LDB.items).Select((int) planetTransport.factory.entityPool[component.entityId].protoId).prefabDesc;
            //
            component.setToggle();
            component.AddShipDrone(prefabDesc);
            component.AddWarperRequestToLastSlot();
            component.SetCharge(prefabDesc);
        }
    }
}