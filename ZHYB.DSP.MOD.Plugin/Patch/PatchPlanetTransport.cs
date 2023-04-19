using System.Collections.Generic;
using System;

using HarmonyLib;
using System.Linq;

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
                return;
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