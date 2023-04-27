﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHYB.DSP.MOD.Plugin.Patch
{
    [HarmonyPatch(typeof(UIMinerWindow))]
    internal class Patch_UIMinerWindow
    {
        private static readonly string _coverText = "zhyb覆盖矿".Translate();
        private static readonly string _veinText = Localization.language==Language.enUS ? " veins" : "个矿脉".Translate();
        private static readonly string _displayUnit = ModConfig.ConfigShowMiner.DisplayAsPerSecond.Value ? "sec" : "min";
        private static readonly float _displayFactor = ModConfig.ConfigShowMiner.DisplayAsPerSecond.Value ? 60.0f : 1.0f;
        private static readonly float _oreValuePerNode = 30.0f;

        [HarmonyPatch("_OnUpdate")]
        private static void Postfix(UIMinerWindow __instance)
        {
            if(__instance.minerId==0||__instance.factory==null)
            {
                __instance._Close();
            }
            else
            {
                MinerComponent minerComponent = __instance.factorySystem.minerPool[__instance.minerId];
                if(minerComponent.id!=__instance.minerId)
                {
                    __instance._Close();
                }
                else
                {
                    if(minerComponent.type==EMinerType.Vein)
                    {
                        var speed = (_oreValuePerNode * GameMain.data.history.miningSpeedScale * minerComponent.veinCount) / _displayFactor;
                        var speedText = speed.ToString("0.##");
                        __instance.coverText.text=$"{_coverText}{minerComponent.veinCount}{_veinText} ({speedText}/{_displayUnit})";
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(UIVeinCollectorPanel))]
    internal static class Patch_UIVeinCollectorPanel__OnUpdate
    {
        private static readonly string _coverText = "zhyb覆盖矿".Translate();

        private static readonly string _veinText = Localization.language==Language.enUS ? " veins" : "个矿脉".Translate();

        private static readonly string _displayUnit = ModConfig.ConfigShowMiner.DisplayAsPerSecond.Value ? "sec" : "min";
        private static readonly float _displayFactor = ModConfig.ConfigShowMiner.DisplayAsPerSecond.Value ? 60.0f : 1.0f;
        private static readonly float _oreValuePerNode = 60.0f;

        [HarmonyPatch("_OnUpdate")]
        private static void Postfix(UIVeinCollectorPanel __instance)
        {
            if(__instance.minerId==0||__instance.factory==null)
            {
                __instance._Close();
            }
            else
            {
                MinerComponent minerComponent = __instance.factorySystem.minerPool[__instance.minerId];
                if(minerComponent.id!=__instance.minerId)
                {
                    __instance._Close();
                }
                else
                {
                    if(minerComponent.type==EMinerType.Vein)
                    {
                        var speed = (_oreValuePerNode * (float)GameMain.data.history.miningSpeedScale * (float)minerComponent.veinCount * ((float)minerComponent.speed / 10000.0)) / _displayFactor;
                        var speedText = speed.ToString("0.##");
                        var coverTextUnlessStacking = minerComponent.productCount > 0 ? "" : _coverText;
                        __instance.coverText.text=$"{coverTextUnlessStacking}{minerComponent.veinCount}{_veinText} ({speedText}/{_displayUnit})";
                    }
                }
            }
        }
    }
}