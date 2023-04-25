namespace ZHYB.DSP.MOD.Plugin
{
    [HarmonyPatch(typeof(BuildTool_Click))]
    public static class Patch_BuildTool_Click
    {
        [HarmonyPostfix]
        [HarmonyPatch("_OnInit")]
        public static void ChangeBuildRange(BuildTool_Click __instance)
        {
            __instance.dotsSnapped=new Vector3[500];
        }
    }

    [HarmonyPatch(typeof(BuildTool_BlueprintPaste))]
    public static class Patch_BuildTool_BlueprintPaste
    {
        [HarmonyPostfix]
        [HarmonyPatch("_OnInit")]
        public static void ChangeBuildRange(BuildTool_BlueprintPaste __instance)
        {
            __instance.dotsSnapped=new Vector3[50];
        }
    }
}