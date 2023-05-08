﻿using System.Globalization;

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

        public static Harmony harmony;

        public void Start()
        {
            ModCommon.ModCommon.logger=base.Logger;
            ModTranslate.Init();
            ModConfig.Init(Config);
            harmony=new Harmony(Plugin_GUID);
            harmony.PatchAll();
        }

        public void Update()
        {
            if(GameMain.localPlanet==null)
                return;

            if(Input.GetKeyDown(KeyCode.L))
            {
                ToggleforceAccMode.Toggle_forceAccMode();
            }
            VeinControl.factory=GameMain.localPlanet.factory;
            KeyboardShortcut shortKey=new KeyboardShortcut(KeyCode.V,
                                                           KeyCode.LeftControl,
                                                           KeyCode.LeftShift,
                                                           KeyCode.LeftAlt);
            if(shortKey.IsDown())
            {
                VeinControl.CheatMode=true;
                VeinControl.ControlVein();
            }
        }

        public void OnDestroy()
        {
            harmony.UnpatchAll();
        }
    }
}