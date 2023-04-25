using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using ModCommon;

using UnityEngine;

namespace AutoFixStationByRecipes
{
    [HarmonyPatch(typeof(UIStationWindow))]
    internal class PatchUIStationWindow
    {
        private static UIButton btn;
        private static UIStationWindow stationWindow;

        [HarmonyPostfix]
        [HarmonyPatch("_OnInit")]
        public static void Patch_OnInit()
        {
            stationWindow=UIRoot.instance.uiGame.stationWindow;
            //navi btn

            btn=Util.MakeSmallTextButton("选择配方",100,0);
            if(btn!=null)
            {
                btn.gameObject.name="ZHYB-DSP-MOD-Plugin-ShowReciper-btn";
                RectTransform rect = Util.NormalizeRectD(btn.gameObject);

                rect.SetParent(stationWindow.windowTrans,false);
                rect.anchoredPosition=new Vector3(400f,-60f);
                btn.highlighted=true;

                btn.onClick+=OnReciperSelectButtonClick;
                btn.tips.tipTitle="ShowReciper";
                btn.tips.tipText="Auto fix Station By Reciper";
                btn.tips.corner=8;
                btn.tips.offset=new Vector2(0f,8f);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("_OnOpen")]
        public static void Patch_OnOpen()
        {
            var stationComponent= stationWindow.transport.stationPool[stationWindow.stationId];
            btn.gameObject.SetActive(!stationComponent.isVeinCollector);
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

        private static void OnRecipePickerReturn(RecipeProto recipeProto)
        {
            if(recipeProto==null)
                return;
            StationComponent component=AutoFixStationByReciper.AutoFixStationByReciper.factory.transport.stationPool[UIRoot.instance.uiGame.stationWindow.stationId];
            if(component==null)
                return;
            int idx  ;
            for(idx=0;idx<component.storage.Length;idx++)
            {
                AutoFixStationByReciper.AutoFixStationByReciper.factory.transport.SetStationStorage(component.id,idx,0,100,ELogisticStorage.Demand,ELogisticStorage.None,GameMain.mainPlayer);
            }

            Dictionary<int,CountItemResult> keyValuePairs=new Dictionary<int, CountItemResult>();
            CountItemResult countItemResult,           tmp;
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
                AutoFixStationByReciper.AutoFixStationByReciper.factory.transport.SetStationStorage(
                    component.id,idx++,
                    keyValue.Key,int.MaxValue,
                    tmp.GetlocalLogic(),
                    tmp.GetRemoteLogic(),
                    GameMain.mainPlayer);
            }

            if(component.isStellar&&( component.storage[component.storage.Length-1].itemId==ItemIds.SpaceWarper||component.storage[component.storage.Length-1].itemId==0 ))
            {
                AutoFixStationByReciper.AutoFixStationByReciper.factory.transport.SetStationStorage(
                       component.id,component.storage.Length-1,
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
                UIRecipePicker.Popup(UIRoot.instance.uiGame.stationWindow.windowTrans.anchoredPosition+new Vector2(-300f,-135f),
                OnRecipePickerReturn,ERecipeType.None);
            }
        }
    }
}