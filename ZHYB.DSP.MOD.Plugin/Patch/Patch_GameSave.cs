using UnityEngine;

namespace Patch
{
	[HarmonyPatch(typeof(GameSave))]
	internal class Patch_GameSave
	{
		public delegate void ActionPlanetData(PlanetData planet);

		public delegate void ActionStarData(StarData star);

		public static List<ActionPlanetData> actonPlanet = new();
		public static List<ActionStarData> actionStar = new();

		public static void setStationComponent(PlanetData planet)
		{
			if(planet.type!=EPlanetType.Gas)
				return;
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
							component.SetCharge(prefabDesc);
						}
					}
				}
			}
		}

		public static void setPowerAccumulatorComponent(PlanetData planet)
		{
			if(planet.type!=EPlanetType.Gas)
				return;
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

		public static void setPlanetName(PlanetData planet)
		{
			if(planet.singularity==EPlanetSingularity.TidalLocked)
			{
				if(planet.name.Contains("潮汐锁定"))
					return;

				planet.overrideName=planet.name+"  潮汐锁定";
				while(GameMain.history.GetStarPin(planet.star.id)!=EPin.Show)
					GameMain.history.ToggleStarPin(planet.star.id);
				while(GameMain.history.GetPlanetPin(planet.id)!=EPin.Show)
					GameMain.history.TogglePlanetPin(planet.id);//.SetPlanetPin(planet.id,EPin.Show);
			}
		}

		[HarmonyPostfix]
		[HarmonyPatch("LoadCurrentGame")]
		public static void LoadCurrentGamePatch(ref bool __result)
		{
			if(!__result)
				return;

			actonPlanet.Add(setStationComponent);
			actonPlanet.Add(setPowerAccumulatorComponent);
			actonPlanet.Add(setPlanetName);

			foreach(StarData star in GameMain.galaxy.stars)
			{
				foreach(PlanetData planet in star.planets)
				{
					foreach(ActionPlanetData item in actonPlanet)
					{
						item(planet);
					}
				}
				foreach(ActionStarData item in actionStar)
				{
					item(star);
				}
			}
		}
	}
}