﻿namespace ZHYB.DSP.MOD.Plugin
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
                ToggleforceAccMode.forceAccMode=false;

            factory=GameMain.localPlanet.factory;

            if(Input.GetKeyDown(KeyCode.L))
            {
                ToggleforceAccMode.Toggle_forceAccMode();
            }
        }

        public void OnDestroy()
        {
            harmony.UnpatchAll();
        }
    }
}