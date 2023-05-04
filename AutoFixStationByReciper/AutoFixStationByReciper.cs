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

        private Harmony harmony;

        public void Start()
        {
            ModTranslate.Init();
            harmony=new Harmony(Plugin_GUID);
            harmony.PatchAll();
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