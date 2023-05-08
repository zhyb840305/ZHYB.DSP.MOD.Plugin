namespace Patch
{
    [HarmonyPatch(typeof(UIStationWindow))]
    internal class PatchUIStationWindow
    {
        private static UIButton btnSelectReciper;
        private static UIStationWindow stationWindow;
        private static StationComponent component;
        private static PlanetFactory factory;
        private static Dictionary<int,CountItemResult> countItemResults = new();

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

                btnSelectReciper.tips.type=UIButton.ItemTipType.Recipe;
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
            component=factory.transport.stationPool[stationWindow.stationId];
            btnSelectReciper.gameObject.SetActive(!component.isVeinCollector);
        }

        private static void OnRecipePickerReturn(RecipeProto recipeProto)
        {
            static int GetStationItemMax()
            {
                int modelIndex = factory.entityPool[component.entityId].modelIndex;
                ModelProto modelProto = LDB.models.Select(modelIndex);
                int num = 0;
                if(modelProto!=null)
                {
                    num=modelProto.prefabDesc.stationMaxItemCount;
                }

                int    itemCountMax=num+
                ( component.isCollector ? GameMain.history.localStationExtraStorage : ( component.isVeinCollector ? GameMain.history.localStationExtraStorage : ( ( !component.isStellar ) ? GameMain.history.localStationExtraStorage : GameMain.history.remoteStationExtraStorage ) ) );
                return itemCountMax;
            }

            if(recipeProto==null)
                return;

            if(component==null)
                return;

            int itemCountMax = GetStationItemMax();

            for(var id = 0;id<component.storage.Length;id++)
            {
                factory.transport.SetStationStorage(
                    component.id,id,
                    0,100,
                    ELogisticStorage.Demand,ELogisticStorage.None,
                    GameMain.mainPlayer);
            }

            RefreshCountItemResult(recipeProto);

            int    idx=0;
            foreach(var keyValue in countItemResults)
            {
                factory.transport.SetStationStorage(
                    component.id,idx++,
                    keyValue.Key,
                    keyValue.Value.GetRemoteLogic()==ELogisticStorage.Demand ? itemCountMax/10 : itemCountMax,
                    keyValue.Value.GetlocalLogic(),
                    keyValue.Value.GetRemoteLogic(),
                    player: GameMain.mainPlayer);
            }

            if(component.isStellar&&( component.storage[component.storage.Length-1].itemId==ItemIds.SpaceWarper||component.storage[component.storage.Length-1].itemId==0 ))
            {
                factory.transport.SetStationStorage(
                    component.id,component.storage.Length-1,
                    ItemIds.SpaceWarper,100,
                    ELogisticStorage.Demand,
                    ELogisticStorage.None,
                    GameMain.mainPlayer);
            }
        }

        private static void RefreshCountItemResult(RecipeProto recipeProto)
        {
            int idx;
            countItemResults.Clear();
            CountItemResult countItemResult, tmp;
            for(idx=0;idx<recipeProto.Items.Count();idx++)
            {
                int itemid=recipeProto.Items[idx]  ;
                int Count=recipeProto.ItemCounts[idx]  ;

                if(countItemResults.TryGetValue(key: itemid,out countItemResult))
                {
                    tmp=countItemResult;
                    tmp.itemCount=Count;
                    countItemResults[itemid]=tmp;
                }
                else
                {
                    tmp.itemCount=Count;
                    tmp.resultCount=0;
                    countItemResults.Add(itemid,tmp);
                }
            }

            for(idx=0;idx<recipeProto.Results.Count();idx++)
            {
                int itemid=recipeProto.Results[idx] ;
                int Count=recipeProto.ResultCounts[idx]      ;

                if(countItemResults.TryGetValue(itemid,out countItemResult))
                {
                    tmp=countItemResult;
                    tmp.resultCount=Count;
                    countItemResults[itemid]=tmp;
                }
                else
                {
                    tmp.itemCount=0;
                    tmp.resultCount=Count;
                    countItemResults.Add(itemid,tmp);
                }
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