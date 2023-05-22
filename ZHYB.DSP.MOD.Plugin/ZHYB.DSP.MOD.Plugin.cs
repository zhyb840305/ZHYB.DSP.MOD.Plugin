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

			Patch_UIStationWindow.AutoPercent_Demand=ModConfig.ConfigAutoFixStationByReciper.AutoPercent_Demand.Value;
			Patch_UIStationWindow.AutoPercent_Supply=ModConfig.ConfigAutoFixStationByReciper.AutoPercent_Supply.Value;

			harmony=new Harmony(Plugin_GUID);
			harmony.PatchAll();
		}

		public void Update()
		{
			if(GameMain.localPlanet==null)
				return;

			ManagerModClass.KeyDown();
		}

		public void OnDestroy()
		{
			harmony.UnpatchSelf();
		}
	}
}