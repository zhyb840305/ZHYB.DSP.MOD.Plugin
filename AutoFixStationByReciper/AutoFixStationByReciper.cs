using Patch;

namespace AutoFixStationByReciper
{
	[BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
	[BepInProcess(Plugin_Process)]
	public class AutoFixStationByReciper:BaseUnityPlugin
	{
		public const string Plugin_GUID = "ZHYB.DSP.MOD.AutoFixStationByReciper";
		public const string Plugin_NAME = "ZHYB.DSP.MOD.AutoFixStationByReciper";
		public const string Plugin_Process = "DSPGAME.exe";
		public const string Plugin_VERSION = "20230420.20.45";
		public static PlanetFactory factory;
		private static readonly string SECTION = "配方配置物流塔";
		private static ConfigEntry<int> AutoPercent_Supply;
		private static ConfigEntry<int> AutoPercent_Demand;
		private Harmony harmony;

		public void Start()
		{
			AutoPercent_Supply=Config.Bind<int>(SECTION,"AutoPercent_Supply",100,
				 new ConfigDescription("根据配方配置物流塔格子,供应（产出物）提供的比例，比如每个格子最大20000，设置成10就是 10/100*20000=2000",new AcceptableValueRange<int>(1,100)));
			AutoPercent_Demand=Config.Bind<int>(SECTION,"AutoPercent_Demand",10,
				new ConfigDescription("根据配方配置物流塔格子,需求（原材料）产品提供的比例，比如每个格子最大20000，设置成10就是 10/100*20000=2000",new AcceptableValueRange<int>(1,100)));
			ModTranslate.Init();

			Patch_UIStationWindow.AutoPercent_Supply=AutoPercent_Supply.Value;
			Patch_UIStationWindow.AutoPercent_Demand=AutoPercent_Demand.Value;
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
			harmony.UnpatchSelf();
		}
	}
}