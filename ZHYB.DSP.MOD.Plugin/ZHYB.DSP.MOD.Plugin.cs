﻿using System.Linq;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using ModCommon;

using UnityEngine;

namespace ZHYB.DSP.MOD.Plugin
{
    [BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
    [BepInProcess(Plugin_Process)]
    public class ModPlugin:BaseUnityPlugin
    {
        public const string Plugin_GUID = "ZHYB.DSP.MOD.Plugin";
        public const string Plugin_NAME = "ZHYB.DSP.MOD.Plugin";
        public const string Plugin_Process = "DSPGAME.exe";
        public const string Plugin_VERSION = "1.0.0";
        public static PlanetFactory factory;
        public static ManualLogSource logger;
        private Harmony harmony;
        private bool forceAccMode = false;

        public void Start()
        {
            logger=base.Logger;
            ModTranslate.Init();
            ModConfig.Init(Config);
            harmony=new Harmony(Plugin_GUID);
            harmony.PatchAll();
        }

        public void Update()
        {
            if(GameMain.localPlanet==null)
                return;

            if(factory!=GameMain.localPlanet.factory)
                forceAccMode=false;

            factory=GameMain.localPlanet.factory;

            if(Input.GetKeyDown(KeyCode.L))
            {
                forceAccMode=!forceAccMode;
                string s= forceAccMode ? "全加速" : "额外生产";
                UIRealtimeTip.Popup("即将设置为:"+s);
                for(var idx = 0;idx<factory.factorySystem.assemblerPool.Count();idx++)
                    if(factory.factorySystem.assemblerPool[idx].id!=0)
                    {
                        if(factory.factorySystem.assemblerPool[idx].productive)
                            factory.factorySystem.assemblerPool[idx].forceAccMode=forceAccMode;
                    }
            }
        }

        public void OnDestroy()
        {
            harmony.UnpatchAll();
        }
    }
}