namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(BuildingParameters))]
    internal class PatchBuildingParameters
    {
        [HarmonyPostfix]
        [HarmonyPatch("ApplyPrebuildParametersToEntity")]
        public static void ApplyPrebuildParametersToEntity(int entityId,int recipeId,int filterId,int[] parameters,PlanetFactory factory)
        {
            int stationId = factory.entityPool[entityId].stationId;
            if(stationId==0)
                return;
            StationComponent component = factory.transport.stationPool[stationId];
            if(component==null)
                return;
            component.setToggle();
        }
    }
}