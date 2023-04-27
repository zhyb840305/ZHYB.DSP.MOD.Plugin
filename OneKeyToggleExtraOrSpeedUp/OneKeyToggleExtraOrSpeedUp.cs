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

        public void Start()
        {
            ModCommon.ModCommon.logger=base.Logger;
            ModCommon.ModCommon.harmony=new Harmony(Plugin_GUID);
            ModCommon.ModCommon.harmony.PatchAll();
        }

        public void Update()
        {
            if(GameMain.localPlanet==null)
                return;
            ModCommon.ModCommon.factory=GameMain.localPlanet.factory;
            if(ModCommon.ModCommon.factory==null)
                return;
            KeyboardShortcut shortcut=new KeyboardShortcut(KeyCode.L,KeyCode.LeftControl );
            if(shortcut.IsDown())
            {
                ZHYB.DSP.MOD.Plugin.ToggleforceAccMode.Toggle_forceAccMode();
            }
        }

        public void OnDestroy()
        {
            ModCommon.ModCommon.harmony.UnpatchAll();
        }
    }
}