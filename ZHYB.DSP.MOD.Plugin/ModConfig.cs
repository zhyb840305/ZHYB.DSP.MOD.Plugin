namespace ModCommon
{
	public static class ModConfig
	{
		public static ConfigEntry<bool> CheatMode;

		internal static void Init(ConfigFile config)
		{
			CheatMode=config.Bind("公共参数","CheatMode",true,description: "启用作弊模式");

			//发电设备
			ConfigPrefabDesc.genEnergyPerTick=config.Bind<int>(ConfigPrefabDesc.SECTION,"genEnergyPerTick",100,"发电倍数");
			ConfigPrefabDesc.powerCoverRadius=config.Bind<int>(ConfigPrefabDesc.SECTION,"powerCoverRadius",2,"覆盖范围");
			ConfigPrefabDesc.powerConnectDistance=config.Bind<int>(ConfigPrefabDesc.SECTION,"powerConnectDistance",2,"链接距离");
			//戴森球半径
			ConfigDysonSphere.maxOrbitRadius=config.Bind<int>(ConfigDysonSphere.SECTION,"maxOrbitRadius",5,"戴森球最大半径倍数");
			//物流塔
			ConfigStationComponent.droneAutoReplenish=config.Bind<bool>(ConfigStationComponent.SECTION,"droneAutoReplenish",false,"自动填充Drone，即自动的把小飞机全部填满");
			ConfigStationComponent.shipAutoReplenish=config.Bind<bool>(ConfigStationComponent.SECTION,"shipAutoReplenish",false,"自动填充Ship，即自动的把大飞机全部填满");
			ConfigStationComponent.warperNecessary=config.Bind<bool>(ConfigStationComponent.SECTION,"warperNecessary",true,"必须Warper");
			//自动配置物流塔格子
			ConfigAutoFixStationByReciper.AutoPercent_Supply=config.Bind<int>(ConfigAutoFixStationByReciper.SECTION,"AutoPercent_Supply",100,
				 new ConfigDescription("根据配方配置物流塔格子,供应（产出物）提供的比例，比如每个格子最大20000，设置成10就是 10/100*20000=2000",new AcceptableValueRange<int>(1,100)));
			ConfigAutoFixStationByReciper.AutoPercent_Demand=config.Bind<int>(ConfigAutoFixStationByReciper.SECTION,"AutoPercent_Demand",10,
				new ConfigDescription("根据配方配置物流塔格子,需求（原材料）产品提供的比例，比如每个格子最大20000，设置成10就是 10/100*20000=2000",new AcceptableValueRange<int>(1,100)));
		}

		public static class ConfigPrefabDesc
		{
			public static string SECTION = "发电设备";
			public static ConfigEntry<int> genEnergyPerTick;
			public static ConfigEntry<int> powerCoverRadius;
			public static ConfigEntry<int> powerConnectDistance;
		}

		public static class ConfigStationComponent
		{
			public static string SECTION = "物流塔";
			public static ConfigEntry<bool> droneAutoReplenish;
			public static ConfigEntry<bool> shipAutoReplenish;
			public static ConfigEntry<bool> warperNecessary;
		}

		public static class ConfigDysonSphere
		{
			public static string SECTION = "戴森球";
			public static ConfigEntry<int> maxOrbitRadius;
		}

		public static class ConfigAutoFixStationByReciper
		{
			public static string SECTION = "配方配置物流塔";
			public static ConfigEntry<int> AutoPercent_Supply;
			public static ConfigEntry<int> AutoPercent_Demand;
		}
	}
}