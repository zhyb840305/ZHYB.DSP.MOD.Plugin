using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using AutoFixStationByRecipes;
using ModCommon;

namespace AutoFixStationByReciper
{
    [BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
    [BepInProcess(Plugin_Process)]
    [BepInIncompatibility(Plugin_GUID)]
    public class AutoFixStationByReciper:BaseUnityPlugin
    {
        public const string Plugin_GUID = "ZHYB.DSP.MOD.AutoFixStationByReciper";
        public const string Plugin_NAME = "ZHYB.DSP.MOD.AutoFixStationByReciper";
        public const string Plugin_Process = "DSPGAME.exe";
        public const string Plugin_VERSION = "20230420.20.45";
        public static PlanetFactory factory;
        public static ManualLogSource logger;
        private Harmony harmony;

        public void Start()
        {
            logger=base.Logger;
            ModTranslate.Init();
            harmony=new Harmony(Plugin_GUID);
            harmony.PatchAll(typeof(PatchUIStationWindow));
        }

        public void Update()
        {
            if(GameMain.localPlanet==null)
                return;
            factory=GameMain.localPlanet.factory;
        }

        public void OnDestroy()
        {
            harmony.UnpatchAll();
        }
    }
}