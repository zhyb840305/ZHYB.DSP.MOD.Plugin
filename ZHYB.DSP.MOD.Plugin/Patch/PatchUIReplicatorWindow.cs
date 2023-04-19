using HarmonyLib;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(UIReplicatorWindow))]
    internal class PatchUIReplicatorWindow
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnPlusButtonClick")]
        private static bool PatchOnPlusButtonClick(UIReplicatorWindow __instance)
        {
            RecipeProto selectedRecipe= Traverse.Create(__instance).Field("selectedRecipe").GetValue() as RecipeProto;
            if(selectedRecipe!=null)
            {
                if(!__instance.multipliers.ContainsKey(selectedRecipe.ID))
                    __instance.multipliers[selectedRecipe.ID]=1;

                int num =__instance. multipliers[selectedRecipe.ID];
                if(num<10)
                    num+=1;
                else if(num<100)
                    num+=10;
                else if(num<1000)
                    num+=100;
                else
                    num+=1000;

                __instance.multipliers[selectedRecipe.ID]=num;
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch("OnMinusButtonClick")]
        private static bool PatchOnMinusButtonClick(UIReplicatorWindow __instance)
        {
            RecipeProto selectedRecipe= Traverse.Create(__instance).Field("selectedRecipe").GetValue() as RecipeProto;
            if(selectedRecipe!=null)
            {
                if(!__instance.multipliers.ContainsKey(selectedRecipe.ID))
                    __instance.multipliers[selectedRecipe.ID]=1;

                int num =__instance. multipliers[selectedRecipe.ID];
                if(num>1000)
                    num-=1000;
                else if(num>100)
                    num-=100;
                else if(num>10)
                    num-=10;
                else if(num>1)
                    num-=1;

                __instance.multipliers[selectedRecipe.ID]=num;
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch("OnOkButtonClick")]
        private static bool PatchOnOkButtonClick(UIReplicatorWindow __instance)
        {
            if(GameMain.sandboxToolsEnabled&&__instance.isInstantItem)
            {
                return true;
            }
            RecipeProto selectedRecipe= Traverse.Create(__instance).Field("selectedRecipe").GetValue() as RecipeProto;
            MechaForge mechaForge=Traverse.Create(__instance).Field("mechaForge").GetValue() as MechaForge;
            if(selectedRecipe==null)
            {
                return false;
            }

            int iD = selectedRecipe.ID;
            int num = 1;
            if(__instance.multipliers.ContainsKey(iD))
            {
                num=__instance.multipliers[iD];
            }
            if(num<1)
            {
                num=1;
            }

            if(!selectedRecipe.Handcraft)
            {
                UIRealtimeTip.Popup("该配方".Translate()+selectedRecipe.madeFromString+"生产".Translate());
            }
            else if(!GameMain.history.RecipeUnlocked(iD))
            {
                UIRealtimeTip.Popup("配方未解锁".Translate());
            }
            else
            {
                int num6 =   mechaForge.PredictTaskCount(selectedRecipe.ID,int.MaxValue);

                if(num>num6)
                    num=num6;

                if(num==0)
                {
                    UIRealtimeTip.Popup("材料不足".Translate());
                }
                else if(mechaForge.AddTask(iD,num)==null)
                {
                    UIRealtimeTip.Popup("材料不足".Translate());
                }
                else
                {
                    GameMain.history.RegFeatureKey(1000104);
                }
            }
            return false;
        }
    }
}