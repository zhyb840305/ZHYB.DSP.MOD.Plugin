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
                    takeItem=5;
                component.idleShipCount=GameMain.mainPlayer.package.TakeItem(ItemIds.LogisticsVessel,takeItem,out _);
                UIRealtimeTip.Popup("大飞机数量："+( ItemCount-takeItem ).ToString());
            }
            component.deliveryShips=100;
        }

        public static void AddWarperRequestToLastSlot(this StationComponent component)
        {
            if(!component.isStellar)
                return;
            ModPlugin.factory.transport.SetStationStorage(
                component.id,
                component.storage.Length-1,
                ItemIds.SpaceWarper,
                100,
                ELogisticStorage.Demand,
                ELogisticStorage.None,
                GameMain.mainPlayer);
            ModPlugin.factory.transport.gameData.galacticTransport.RefreshTraffic(component.gid);
        }

        public static void SetCharge(this StationComponent component,PrefabDesc prefabDesc)
        {
            ModPlugin.factory.powerSystem.consumerPool[component.pcId].workEnergyPerTick=prefabDesc.workEnergyPerTick*50;
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