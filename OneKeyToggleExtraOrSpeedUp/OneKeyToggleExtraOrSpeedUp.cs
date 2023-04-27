﻿namespace OneKeyToggleExtraOrSpeedUp
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

        public void Start()
        {
            logger=base.Logger;
            harmony=new Harmony(Plugin_GUID);
            harmony.PatchAll();
        }

        public void Update()
        {
            if(GameMain.localPlanet==null)
                return;
            factory=GameMain.localPlanet.factory;
            if(factory==null)
                return;
            KeyboardShortcut shortcut=new KeyboardShortcut(KeyCode.L,KeyCode.LeftControl );
            if(shortcut.IsDown())
            {
                ZHYB.DSP.MOD.Plugin.ToggleforceAccMode.Toggle_forceAccMode();
            }
        }

        public void OnDestroy()
        {
            harmony.UnpatchAll();
        }
    }
}