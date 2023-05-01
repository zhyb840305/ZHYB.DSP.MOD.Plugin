using BepInEx.Logging;
using BepInEx;

using HarmonyLib;
using Steamworks;
using UnityEngine;
using System.Security.Cryptography;
using System;

namespace SuperAccumulator
{
    [BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
    [BepInProcess(Plugin_Process)]
    public class SuperAccumulator:BaseUnityPlugin
    {
        public const string Plugin_GUID = "ZHYB.DSP.MOD.SuperAccumulator";
        public const string Plugin_NAME = "ZHYB.DSP.MOD.SuperAccumulator";
        public const string Plugin_Process = "DSPGAME.exe";
        public const string Plugin_VERSION = "20230501.9.43";

        private Harmony harmony;
        public static ManualLogSource logger;

        public void Start()
        {
            logger=base.Logger;
            harmony=new Harmony(Plugin_GUID);
            harmony.PatchAll();
        }

        public void OnDestroy()
        {
            harmony.UnpatchAll();
        }
    }

    [HarmonyPatch(typeof(PrefabDesc))]
    internal class PatchPrefabDesc
    {
        [HarmonyPostfix, HarmonyPatch("ReadPrefab")]
        public static void PatchReadPrefab(PrefabDesc __instance)
        {
            //超级电池
            if(__instance.isAccumulator)
            {
                __instance.inputEnergyPerTick=( long )1000*1000*1000*1000;
                __instance.outputEnergyPerTick=( long )1000*1000*1000*1000;
                __instance.maxAcuEnergy=( long )1000*1000*1000*1000*1000*1000;
            }
        }
    }
}