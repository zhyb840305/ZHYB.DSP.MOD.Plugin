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

			PatchUIStationWindow.SetAutoPercent(Demand: ModConfig.ConfigAutoFixStationByReciper.AutoPercent_Demand.Value,Supply: ModConfig.ConfigAutoFixStationByReciper.AutoPercent_Supply.Value);

			harmony=new Harmony(Plugin_GUID);
			harmony.PatchAll();
		}

		public void Update()
		{
			if(GameMain.localPlanet==null)
				return;

			VeinControl.factory=GameMain.localPlanet.factory;
			KeyboardShortcut shortKey_Toggle_forceAccMode=new(KeyCode.L,KeyCode.LeftControl, KeyCode.LeftShift,KeyCode.LeftAlt);
			if(shortKey_Toggle_forceAccMode.IsDown())
			{
				ToggleforceAccMode.Toggle_forceAccMode();
			}

			KeyboardShortcut shortcut_AutoBuild=new(KeyCode.B,KeyCode.LeftControl );
			if(shortcut_AutoBuild.IsDown())
			{
				AutoBuild.AutoBuildEnabled=!AutoBuild.AutoBuildEnabled;
			}

			KeyboardShortcut shortKey_VeinControl=new(KeyCode.V,KeyCode.LeftControl, KeyCode.LeftShift,KeyCode.LeftAlt);
			if(shortKey_VeinControl.IsDown())
			{
				VeinControl.CheatMode=true;
				VeinControl.ControlVein();
			}
			KeyboardShortcut shortcut_TestUIDysonEditor=new KeyboardShortcut(KeyCode.N,KeyCode.LeftControl);
			if(shortcut_TestUIDysonEditor.IsDown())
			{
				TestUIDysonEditor.TestdysonEditor();
			}
		}

		public void OnDestroy()
		{
			harmony.UnpatchSelf();
		}
	}
}