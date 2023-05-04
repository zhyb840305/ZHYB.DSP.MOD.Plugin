﻿namespace Patch
{
    [HarmonyPatch(typeof(UIStationWindow))]
    internal class PatchUIStationWindow
    {
        private static UIButton btnSelectReciper;
        private static UIStationWindow stationWindow;
        private static StationComponent stationComponent;
        private static PlanetFactory factory;

        [HarmonyPostfix]
        [HarmonyPatch("_OnInit")]
        public static void Patch_OnInit()
        {
            stationWindow=UIRoot.instance.uiGame.stationWindow;
            //navi btn

            btnSelectReciper=Util.MakeSmallTextButton("选择配方",100,0);
            if(btnSelectReciper!=null)
            {
                btnSelectReciper.gameObject.name="ZHYB-DSP-MOD-Plugin-ShowReciper-btn";
                RectTransform rect = Util.NormalizeRectD(btnSelectReciper.gameObject);

                rect.SetParent(stationWindow.windowTrans,false);
                rect.anchoredPosition=new Vector3(400f,-60f);
                btnSelectReciper.highlighted=true;

                btnSelectReciper.onClick+=OnReciperSelectButtonClick;
                btnSelectReciper.tips.tipTitle="ShowReciper";
                btnSelectReciper.tips.tipText="Auto fix Station By Reciper";
                btnSelectReciper.tips.corner=8;
                btnSelectReciper.tips.offset=new Vector2(0f,8f);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("_OnOpen")]
        public static void Patch_OnOpen()
        {
            factory=GameMain.localPlanet.factory;
            stationComponent=stationWindow.transport.stationPool[stationWindow.stationId];
            btnSelectReciper.gameObject.SetActive(!stationComponent.isVeinCollector);
        }

        private static void OnRecipePickerReturn(RecipeProto recipeProto)
        {
            if(recipeProto==null)
                return;

            if(stationComponent==null)
                return;

            int idx  ;
            for(idx=0;idx<stationComponent.storage.Length;idx++)
            {
                factory.transport.SetStationStorage(
                              stationComponent.id,idx,
                              0,100,
                              ELogisticStorage.Demand,ELogisticStorage.None,
                              GameMain.mainPlayer);
            }

            Dictionary<int,CountItemResult> keyValuePairs=new Dictionary<int, CountItemResult>();
            CountItemResult countItemResult,  tmp;
            for(idx=0;idx<recipeProto.Items.Count();idx++)
            {
                int itemid=recipeProto.Items[idx]  ;
                int Count=recipeProto.ItemCounts[idx]  ;

                if(keyValuePairs.TryGetValue(key: itemid,out countItemResult))
                {
                    tmp=countItemResult;
                    tmp.itemCount=Count;
                    keyValuePairs[itemid]=tmp;
                }
                else
                {
                    tmp.itemCount=Count;
                    tmp.resultCount=0;
                    keyValuePairs.Add(itemid,tmp);
                }
            }

            for(idx=0;idx<recipeProto.Results.Count();idx++)
            {
                int itemid=recipeProto.Results[idx] ;
                int Count=recipeProto.ResultCounts[idx]      ;

                if(keyValuePairs.TryGetValue(itemid,out countItemResult))
                {
                    tmp=countItemResult;
                    tmp.resultCount=Count;
                    keyValuePairs[itemid]=tmp;
                }
                else
                {
                    tmp.itemCount=0;
                    tmp.resultCount=Count;
                    keyValuePairs.Add(itemid,tmp);
                }
            }

            idx=0;
            foreach(var keyValue in keyValuePairs)
            {
                tmp=keyValue.Value;
                factory.transport.SetStationStorage(
                    stationComponent.id,idx++,
                    keyValue.Key,int.MaxValue,
                    tmp.GetlocalLogic(),
                     tmp.GetRemoteLogic(),
                    GameMain.mainPlayer);
            }

            if(stationComponent.isStellar&&( stationComponent.storage[stationComponent.storage.Length-1].itemId==ItemIds.SpaceWarper||stationComponent.storage[stationComponent.storage.Length-1].itemId==0 ))
            {
                factory.transport.SetStationStorage(
                        stationComponent.id,stationComponent.storage.Length-1,
                        ItemIds.SpaceWarper,100,
                        ELogisticStorage.Demand,ELogisticStorage.None,
                        GameMain.mainPlayer);
            }
        }

        private static void OnReciperSelectButtonClick(int obj)
        {
            if(UIRecipePicker.isOpened)
            {
                UIRecipePicker.Close();
            }
            else
            {
                UIRecipePicker.Popup(stationWindow.windowTrans.anchoredPosition+new Vector2(-300f,-135f),
                OnRecipePickerReturn,ERecipeType.None);
            }
        }

        private struct CountItemResult
        {
            public int itemCount;
            public int resultCount;

            public ELogisticStorage GetlocalLogic()
            {
                return itemCount==0 ? ELogisticStorage.Supply : ( itemCount>resultCount ? ELogisticStorage.Demand : ELogisticStorage.Supply );
            }

            public ELogisticStorage GetRemoteLogic()
            {
                return itemCount==0 ? ELogisticStorage.Supply : ( itemCount>resultCount ? ELogisticStorage.Demand : ELogisticStorage.Supply );
            }
        }
    }
}