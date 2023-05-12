using System.Globalization;

using Patch;

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
			PatchUIStationWindow.AutoPercent_Demand=ModConfig.ConfigAutoFixStationByReciper.AutoPercent_Demand.Value;
			PatchUIStationWindow.AutoPercent_Supply=ModConfig.ConfigAutoFixStationByReciper.AutoPercent_Supply.Value;
			harmony=new Harmony(Plugin_GUID);
			harmony.PatchAll();
		}

		public void Update()
		{
			if(GameMain.localPlanet==null)
				return;

			VeinControl.factory=GameMain.localPlanet.factory;
			KeyboardShortcut shortKey_Toggle_forceAccMode=new KeyboardShortcut(KeyCode.L,KeyCode.LeftControl, KeyCode.LeftShift,KeyCode.LeftAlt);
			if(shortKey_Toggle_forceAccMode.IsDown())
			{
				ToggleforceAccMode.Toggle_forceAccMode();
			}

			KeyboardShortcut shortKey_VeinControl=new KeyboardShortcut(KeyCode.V,KeyCode.LeftControl, KeyCode.LeftShift,KeyCode.LeftAlt);
			if(shortKey_VeinControl.IsDown())
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