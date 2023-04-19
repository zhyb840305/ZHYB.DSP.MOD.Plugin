using BepInEx.Configuration;

namespace ZHYB.DSP.MOD.Plugin
{
    public static class ModConfig
    {
        internal static void Init(ConfigFile config)
        {
            ModPlugin.logger.LogInfo("ModConfig   Init");
            //发电设备
            ConfigPrefabDesc.genEnergyPerTick=config.Bind<int>(
                ConfigPrefabDesc.SECTION,"genEnergyPerTick",100,"发电倍数");
            ConfigPrefabDesc.powerCoverRadius=config.Bind<int>(
                ConfigPrefabDesc.SECTION,"powerCoverRadius",2,"覆盖范围");
            ConfigPrefabDesc.powerConnectDistance=config.Bind<int>(
                ConfigPrefabDesc.SECTION,"powerConnectDistance",1,"链接距离");
            //戴森球半径
            ConfigDysonSphere.maxOrbitRadius=config.Bind<int>(
                ConfigDysonSphere.SECTION,"maxOrbitRadius",5,"戴森球最大半径倍数");
            //物流塔
            ConfigStationComponent.droneAutoReplenish=config.Bind<bool>(
                ConfigStationComponent.SECTION,"droneAutoReplenish",false,"自动填充Drone");
            ConfigStationComponent.shipAutoReplenish=config.Bind<bool>(
                ConfigStationComponent.SECTION,"shipAutoReplenish",false,"自动填充Ship");
            ConfigStationComponent.warperNecessary=config.Bind<bool>(
                ConfigStationComponent.SECTION,"warperNecessary",true,"必须Warper");
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
    }
}