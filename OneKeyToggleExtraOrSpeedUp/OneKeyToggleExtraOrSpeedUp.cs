using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using UnityEngine;

namespace OneKeyToggleExtraOrSpeedUp
{
    [BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
    [BepInProcess(Plugin_Process)]
    public class ModPlugin:BaseUnityPlugin
    {
        public const string Plugin_GUID = "ZHYB.DSP.MOD.OneKeyToggleExtraOrSpeedUp";
        public const string Plugin_NAME = "ZHYB.DSP.MOD.OneKeyToggleExtraOrSpeedUp";
        public const string Plugin_Process = "DSPGAME.exe";
        public const string Plugin_VERSION = "1.0.0";
        public static PlanetFactory factory;
        public static ManualLogSource logger;
        private Harmony harmony;
        private static bool forceAccMode;

        public void Start()
        {
            logger=base.Logger;
            harmony=new Harmony(Plugin_GUID);
            harmony.PatchAll();
            forceAccMode=false;
        }

        public void Update()
        {
            if(GameMain.localPlanet==null)
                return;
            factory=GameMain.localPlanet.factory;
            if(factory==null)
                return;
            if(Input.GetKeyDown(KeyCode.L))
            {
                forceAccMode=!forceAccMode;
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