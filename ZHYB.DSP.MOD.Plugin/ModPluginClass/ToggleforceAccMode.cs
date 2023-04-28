namespace ModClass
{
    internal static class ToggleforceAccMode
    {
        public static bool forceAccMode = false;
        public static PlanetFactory factory = null;

        public static void Toggle_forceAccMode()
        {
            if(GameMain.localPlanet==null)
                return;

            if(factory!=GameMain.localPlanet.factory)
                forceAccMode=false;

            factory=GameMain.localPlanet.factory;
            if(factory==null)
                return;
            forceAccMode=!forceAccMode;
            string s= forceAccMode ? "全加速" : "额外生产";
            UIRealtimeTip.Popup("即将将本星求全部设置为:"+s);
            for(var idx = 0;idx<factory.factorySystem.assemblerPool.Count();idx++)
                if(factory.factorySystem.assemblerPool[idx].id!=0)
                {
                    if(factory.factorySystem.assemblerPool[idx].productive)
                        factory.factorySystem.assemblerPool[idx].forceAccMode=forceAccMode;
                }
            UIRealtimeTip.Popup("设置完毕，目前为："+s);
        }
    }
}