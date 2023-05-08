namespace OneKeyToggleExtraOrSpeedUp
{
	[BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
	[BepInProcess(Plugin_Process)]
	public class ModPlugin:BaseUnityPlugin
	{
		public const string Plugin_GUID = "ZHYB.DSP.MOD.OneKeyToggleExtraOrSpeedUp";
		public const string Plugin_NAME = "ZHYB.DSP.MOD.OneKeyToggleExtraOrSpeedUp";
		public const string Plugin_Process = "DSPGAME.exe";
		public const string Plugin_VERSION = "20230508.22.55";

		private Harmony harmony;

		public void Start()
		{
			ModCommon.ModCommon.logger=base.Logger;
			harmony=new Harmony(Plugin_GUID);
			harmony.PatchAll();
		}

		public void Update()
		{
			if(GameMain.localPlanet==null)
				return;

			KeyboardShortcut shortcut=new KeyboardShortcut(KeyCode.L,KeyCode.LeftControl,KeyCode.LeftShift,KeyCode.LeftAlt );
			if(shortcut.IsDown())
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