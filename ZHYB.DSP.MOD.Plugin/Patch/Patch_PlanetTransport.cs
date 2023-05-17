namespace Patch
{
    [HarmonyPatch(typeof(PlanetTransport))]
    internal class Patch_PlanetTransport
    {
        [HarmonyPostfix]
        [HarmonyPatch("NewStationComponent")]
        public static void PatchNewStationComponent(PlanetTransport __instance,StationComponent __result)
        {
            StationComponent component = __result;

            if(component.isCollector)
                return;
            PlanetTransport planetTransport = __instance;
            PrefabDesc prefabDesc = LDB.items.Select( planetTransport.factory.entityPool[component.entityId].protoId).prefabDesc;

            component.setToggle();
            component.AddShipDrone(prefabDesc);
            component.AddWarperRequestToLastSlot();
            component.SetCharge(prefabDesc);
        }
    }
}