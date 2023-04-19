using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace ZHYB.DSP.MOD.Plugin
{
    [BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
    [BepInProcess(Plugin_Process)]
    public class ModPlugin:BaseUnityPlugin
    {
        public const string Plugin_GUID = "ZHYB.DSP.MOD.Plugin";
        public const string Plugin_NAME = "ZHYB.DSP.MOD.Plugin";
        public const string Plugin_Process = "DSPGAME.exe";
        public const string Plugin_VERSION = "1.0";
        public static PlanetFactory factory;
        public static ManualLogSource logger;

        public void Start()
        {
            logger=base.Logger;
            ModConfig.Init(Config);
            Harmony harmony = new Harmony(Plugin_GUID);
            harmony.PatchAll();
        }

        public void Update()
        {
            if(GameMain.localPlanet==null)
                return;
            factory=GameMain.localPlanet.factory;
        }
    }
}