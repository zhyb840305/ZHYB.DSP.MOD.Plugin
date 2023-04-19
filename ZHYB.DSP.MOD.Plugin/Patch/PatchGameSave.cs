using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(GameSave))]
    internal class PatchGameSave
    {
        [HarmonyPostfix]
        [HarmonyPatch("LoadCurrentGame")]
        public static void LoadCurrentGamePatch(ref bool __result)
        {
            if(!__result)
                return;

            foreach(StarData star in GameMain.galaxy.stars)
            {
                foreach(PlanetData planet in star.planets)
                {
                    PlanetFactory factory = planet?.factory;
                    if(factory!=null)
                    {
                        StationComponent[] stationPool = planet.factory.transport.stationPool;
                        if(stationPool!=null&&stationPool.Length!=0)
                        {
                            for(int stationId = 0;stationId<stationPool.Length;++stationId)
                            {
                                StationComponent component = stationPool[stationId];
                                if(component!=null)
                                {
                                    component.setToggle();
                                    component.SetItemMax();
                                    factory.transport.gameData.galacticTransport.RefreshTraffic(component.gid);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}