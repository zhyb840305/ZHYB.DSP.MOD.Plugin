﻿namespace VeinManager
{
	[BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
	[BepInProcess(Plugin_Process)]
	public class VeinManager:BaseUnityPlugin
	{
		public const string Plugin_GUID = "ZHYB.DSP.MOD.VeinManager";
		public const string Plugin_NAME = "ZHYB.DSP.MOD.VeinManager";
		public const string Plugin_Process = "DSPGAME.exe";
		public const string Plugin_VERSION = "20230507.1.52";

		public static Harmony harmony;
		public ConfigEntry<bool> CheatMode;
		public ConfigEntry<KeyboardShortcut> shortKey;

		public void Start()
		{
			ModCommon.ModCommon.logger=base.Logger;
			CheatMode=Config.Bind("VeinManager","CheatMode",true,
				"是否启用《全矿物添加》模式，启用《全矿物添加》模式后，添加全部矿物，每样矿物1000*10*10000份");
			shortKey=Config.Bind<KeyboardShortcut>("VeinManager","shortKey",
				new KeyboardShortcut(KeyCode.V,KeyCode.LeftControl,KeyCode.LeftShift,KeyCode.LeftAlt),
				"因为一个星球只可能用一次，快捷键设置：LeftControl LeftAlt LeftShift V");

			ModTranslate.Init();
			harmony=new Harmony(Plugin_GUID);
			harmony.PatchAll();
		}

		public void Update()
		{
			if(GameMain.localPlanet==null)
				return;

			if(shortKey.Value.IsDown())
			{
				ModCommon.ModCommon.logger.LogInfo(data: " VeinManager  VeinControl");
				VeinControl.CheatMode=CheatMode.Value;
				VeinControl.ControlVein();
			}
		}

		public void OnDestroy()
		{
			harmony.UnpatchSelf();
		}
	}
}