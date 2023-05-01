using ZHYB.DSP.MOD.Plugin;

using static Patch.PatchGameSave;

namespace Patch
{
    [HarmonyPatch(typeof(GameSave))]
    internal class PatchGameSave
    {
        public delegate void ActionPlanetData(PlanetData planet);

        public static List<ActionPlanetData> actonPlanetDatas = new List<ActionPlanetData>();

        public static void setStationComponent(PlanetData planet)
        {
            ModPlugin.logger.LogInfo("setStationComponent");
            PrefabDesc prefabDesc_PlanetaryLogisticsStation =  LDB.items.Select(ItemIds.PlanetaryLogisticsStation  ).prefabDesc;
            PrefabDesc prefabDesc_InterstellarLogisticsStation=  LDB.items.Select(ItemIds.InterstellarLogisticsStation  ).prefabDesc;

            PlanetFactory factory = planet?.factory;
            if(factory!=null)
            {
                StationComponent[] stationPool = planet.factory.transport.stationPool;
                var   consumerPool=  factory.powerSystem.consumerPool;
                if(stationPool!=null&&stationPool.Length!=0)
                {
                    for(int stationId = 0;stationId<stationPool.Length;++stationId)
                    {
                        StationComponent component = stationPool[stationId];
                        if(component!=null)
                        {
                            component.setToggle();
                            var prefabDesc =component.isStellar?prefabDesc_InterstellarLogisticsStation:prefabDesc_PlanetaryLogisticsStation;
                            if(component.energyMax==prefabDesc.stationMaxEnergyAcc)
                                continue;
                            consumerPool[component.pcId].workEnergyPerTick=prefabDesc.workEnergyPerTick*5;
                            component.energyMax=prefabDesc.stationMaxEnergyAcc;
                            component.energy=component.energyMax;
                        }
                    }
                }
            }
        }

        public static void setPowerAccumulatorComponent(PlanetData planet)
        {
            ModPlugin.logger.LogInfo("setPowerAccumulatorComponent");
            PlanetFactory factory = planet?.factory;
            if(factory==null)
                return;
            PowerAccumulatorComponent[]  accPool=factory.powerSystem.accPool;
            if(accPool!=null&&accPool.Length!=0)
            {
                for(int accid = 0;accid<accPool.Length;++accid)
                {
                    var acc = accPool[accid];
                    if(acc.id!=0)
                        acc.curEnergy=acc.maxEnergy;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("LoadCurrentGame")]
        public static void LoadCurrentGamePatch(ref bool __result)
        {
            if(!__result)
                return;

            actonPlanetDatas.Add(setStationComponent);
            actonPlanetDatas.Add(setPowerAccumulatorComponent);

            foreach(StarData star in GameMain.galaxy.stars)
            {
                foreach(PlanetData planet in star.planets)
                    if(planet.type!=EPlanetType.Gas)
                    {
                        foreach(var actionPlanetData in actonPlanetDatas)
                            actionPlanetData(planet);
                    }
            }
        }
    }
}