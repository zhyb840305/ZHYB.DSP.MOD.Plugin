namespace ModClass
{
	public static class ExtenisStationComponent
	{
		public static void AddShipDrone(this StationComponent component,PrefabDesc prefabDesc)
		{
			int takeItem, NeedCount, ItemCount;
			ItemCount=GameMain.mainPlayer.package.GetItemCount(ItemIds.LogisticsDrone);
			NeedCount=prefabDesc.stationMaxDroneCount;
			if(ItemCount>0)
			{
				if(ItemCount>NeedCount*10)
					takeItem=NeedCount;
				else if(ItemCount>NeedCount*5)
					takeItem=NeedCount/5;
				else
					takeItem=5;
				component.idleDroneCount=GameMain.mainPlayer.package.TakeItem(ItemIds.LogisticsDrone,takeItem,out _);
				UIRealtimeTip.Popup("小飞机数量："+( ItemCount-takeItem ).ToString());
			}
			component.deliveryDrones=100;

			if(!component.isStellar)
				return;

			ItemCount=GameMain.mainPlayer.package.GetItemCount(ItemIds.LogisticsVessel);
			NeedCount=prefabDesc.stationMaxShipCount;
			if(ItemCount>0)
			{
				if(ItemCount>NeedCount*10)
					takeItem=NeedCount;
				else if(ItemCount>NeedCount*5)
					takeItem=NeedCount/5;
				else
					takeItem=2;
				component.idleShipCount=GameMain.mainPlayer.package.TakeItem(ItemIds.LogisticsVessel,takeItem,out _);
				UIRealtimeTip.Popup("大飞机数量："+( ItemCount-takeItem ).ToString());
			}
			component.deliveryShips=100;
		}

		public static void AddWarperRequestToLastSlot(this StationComponent component)
		{
			if(!component.isStellar)
				return;
			PlanetFactory factory = GameMain.localPlanet.factory;
			if(component.storage[component.storage.Length-1].itemId==0&&!HasItemInAnySlot(component,ItemIds.SpaceWarper))
			{
				factory.transport.SetStationStorage(
				   component.id,
				   component.storage.Length-1,
				   ItemIds.SpaceWarper,
				   100,
				   ELogisticStorage.Demand,
				   ELogisticStorage.None,
				   GameMain.mainPlayer);
				factory.transport.gameData.galacticTransport.RefreshTraffic(component.gid);
			}
		}

		public static void SetCharge(this StationComponent component,PrefabDesc prefabDesc)
		{
			GameMain.localPlanet.factory.powerSystem.consumerPool[component.pcId].workEnergyPerTick=prefabDesc.workEnergyPerTick*5;
			//component.energyPerTick=prefabDesc.workEnergyPerTick*25;
			component.energyMax=prefabDesc.stationMaxEnergyAcc;
			component.energy=component.energyMax;
		}

		public static void setToggle(this StationComponent component)
		{
			component.warperNecessary=ModConfig.ConfigStationComponent.warperNecessary.Value;
			component.droneAutoReplenish=ModConfig.ConfigStationComponent.droneAutoReplenish.Value;
			component.shipAutoReplenish=ModConfig.ConfigStationComponent.shipAutoReplenish.Value;
			component.includeOrbitCollector=true;
		}

		private static bool HasItemInAnySlot(this StationComponent component,int itemId) =>
			( ( IEnumerable<StationStore> )component.storage ).Any<StationStore>(( Func<StationStore,bool> )( storage => storage.itemId==itemId ));
	}
}