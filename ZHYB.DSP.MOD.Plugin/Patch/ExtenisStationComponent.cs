using System;
using System.Collections.Generic;
using System.Linq;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
    public static class ExtenisStationComponent
    {
        public static void AddShipDrone(this StationComponent component,PrefabDesc prefabDesc)
        {
            int takeItem, NeedCount, ItemCount;
            ItemCount=GameMain.mainPlayer.package.GetItemCount(ItemIds.Drone);
            NeedCount=prefabDesc.stationMaxDroneCount;
            if(ItemCount>0)
            {
                if(ItemCount>NeedCount*10)
                    takeItem=NeedCount;
                else if(ItemCount>NeedCount*5)
                    takeItem=NeedCount/5;
                else
                    takeItem=5;
                component.idleDroneCount=GameMain.mainPlayer.package.TakeItem(ItemIds.Drone,takeItem,out _);
            }
            component.deliveryDrones=100;

            if(!component.isStellar)
                return;

            ItemCount=GameMain.mainPlayer.package.GetItemCount(ItemIds.Ship);
            NeedCount=prefabDesc.stationMaxShipCount;
            if(ItemCount>0)
            {
                if(ItemCount>NeedCount*10)
                    takeItem=NeedCount;
                else if(ItemCount>NeedCount*5)
                    takeItem=NeedCount/5;
                else
                    takeItem=5;
                component.idleShipCount=GameMain.mainPlayer.package.TakeItem(ItemIds.Ship,takeItem,out _);
            }
            component.deliveryShips=100;
        }

        public static void SetCharge(this StationComponent component,PrefabDesc prefabDesc)
        {
            ModPlugin.factory.powerSystem.consumerPool[component.pcId].workEnergyPerTick=prefabDesc.workEnergyPerTick*5;
        }

        public static void SetItemMax(this StationComponent component)
        {
            for(var storageindex = 0;storageindex<=component.storage.Length-1;++storageindex)
            {
                if(component.storage[storageindex].itemId!=0&&component.storage[storageindex].itemId!=ItemIds.Warper)
                {
                    ModPlugin.factory.transport.SetStationStorage(
                           component.id,storageindex,
                           component.storage[storageindex].itemId,int.MaxValue,
                           component.storage[storageindex].localLogic,component.storage[storageindex].remoteLogic,
                           GameMain.mainPlayer);
                }
                else if(component.isStellar&&( component.storage[storageindex].itemId==ItemIds.Warper||component.storage[storageindex].itemId==0 )&&storageindex==component.storage.Length-1)
                {
                    ModPlugin.factory.transport.SetStationStorage(
                           component.id,storageindex,
                           ItemIds.Warper,100,
                           ELogisticStorage.Demand,ELogisticStorage.None,
                           GameMain.mainPlayer);
                }
            }
        }

        public static void setToggle(this StationComponent component)
        {
            component.warperNecessary=ModConfig.ConfigStationComponent.warperNecessary.Value;
            component.droneAutoReplenish=ModConfig.ConfigStationComponent.droneAutoReplenish.Value;
            component.shipAutoReplenish=ModConfig.ConfigStationComponent.shipAutoReplenish.Value;
            component.includeOrbitCollector=true;
        }

        public static void AddWarperRequestToLastSlot(this StationComponent component)
        {
            if(!component.isStellar)
                return;
            ModPlugin.factory.transport.SetStationStorage(
                component.id,
                component.storage.Length-1,
                ItemIds.Warper,
                100,
                ELogisticStorage.Demand,
                ELogisticStorage.None,
                GameMain.mainPlayer);
            ModPlugin.factory.transport.gameData.galacticTransport.RefreshTraffic(component.gid);
        }

        private static bool HasItemInAnySlot(this StationComponent component,int itemId) =>
            ( ( IEnumerable<StationStore> )component.storage ).Any<StationStore>(( Func<StationStore,bool> )( storage => storage.itemId==itemId ));
    }
}